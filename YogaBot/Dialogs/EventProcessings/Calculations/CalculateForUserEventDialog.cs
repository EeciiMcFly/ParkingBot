using System.Collections;
using System.Data.Common;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.InfoProvider;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.EventProcessings.Calculations;

public class CalculateForAllDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;
    private readonly IUsersRepository usersRepository;
    private readonly IPresenceRepository presenceRepository;
    private readonly TelegramBotClient telegramBotClient;
    private readonly IInfoProvider infoProvider;

    public CalculateForAllDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter,
        IEventsRepository eventsRepository, IArrangementRepository arrangementRepository, IUsersRepository usersRepository, IPresenceRepository presenceRepository, 
        TelegramBotClient telegramBotClient, IUserArrangementRelationsRepository userArrangementRelationsRepository, IInfoProvider infoProvider)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.presenceRepository = presenceRepository;
        this.telegramBotClient = telegramBotClient;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
        this.infoProvider = infoProvider;
    }

    public void StartDialog(BotContext context)
    {
        var message = "Введите ник пользователя, дату начала и дату конца в формате: \n@UserName \n10.03.2022 \n17.03.2022";
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageText = message,
        };

        //TODO userId вместо TelegramUserId
        dialogStateSetter.SetState(context.TelegramUserId, ctx => ProcessArrangementData(ctx, context.CallbackData));
        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;

        return context.CallbackData.Contains(CallbackDataConstants.CalculatePriceForRequester);
    }

    public int Priority => 10;

    private async void ProcessArrangementData(BotContext context, string callbackData)
    {
        var arrangementGuid = Int64.Parse(callbackData.Split('/')[1]);
        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
        var splittedText = context.MessageText.Split("\n");
        var relations = userArrangementRelationsRepository.GetRelationsForArrangementAsync(arrangementGuid);

        var startDate = DateTime.Parse(splittedText[1]).ToUniversalTime();
        var endDate = DateTime.Parse(splittedText[2]).ToUniversalTime();
        
        var events = (await eventsRepository.GetEventsForPeriodAndArrangementAsync(startDate, endDate, arrangementGuid)).ToArray();
        var presencesGroupedByEvent = await Task.WhenAll(events.Select(async x => await presenceRepository.GetPresencesForEventAsync(x.EventId)));
        
        var visitors = presencesGroupedByEvent.SelectMany(x => x).Select(x => x.User).GroupBy(x => x.UserId).Select(x => x.First());

        var costsPerUsers = await Task.WhenAll(visitors.Select(async user =>
        {
            int sum = 0;
            foreach (var @event in events)
            {
                var presences = await presenceRepository.GetPresencesForEventAsync(@event.EventId);
                sum += @event.Cost / presences.Count();
            }

            return new CostPerUser
            {
                cost = sum,
                telegramUsername = (await infoProvider.GetChatMember(arrangement.ChatId, user.TelegramUserId)).User.Username
            };
        }));
        
        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData("Запланировать занятие", CallbackDataConstants.CreateEvent + '/' + arrangementGuid)},
            new[] {InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия", CallbackDataConstants.GetEvents + '/' + arrangementGuid)},
            new[] {InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities)}
        });

        var resultStrings = costsPerUsers.Select(x => x.telegramUsername + " " + Math.Round(x.cost, 2));
        var message = "";

        foreach (var resultString in resultStrings)
        {
            message += "\n" + resultString;
        }

        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
            MessageText = message,
            Ikm = ikm
        };

        outputMessageQueue.AddMessage(answer);
    }

    private class CostPerUser
    {
        public double cost;
        public string telegramUsername;
    }
}