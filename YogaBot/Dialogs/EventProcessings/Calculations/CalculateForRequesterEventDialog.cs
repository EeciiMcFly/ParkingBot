﻿using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.EventProcessings.Calculations;

public class CalculateForRequesterEventDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUsersRepository usersRepository;
    private readonly IPresenceRepository presenceRepository;
    private readonly IKeyboardBuilder keyboardBuilder;

    public CalculateForRequesterEventDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter, 
        IEventsRepository eventsRepository, IArrangementRepository arrangementRepository, IUsersRepository usersRepository, IPresenceRepository presenceRepository, IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.presenceRepository = presenceRepository;
        this.keyboardBuilder = keyboardBuilder;
    }

    public void StartDialog(BotContext context)
    {
        var message = "Введите дату начала и дату конца в формате: \n10.03.2023 \n17.03.2023";
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
        var splittedText = context.MessageText.Split("\n");
        var startDate = DateTime.Parse(splittedText[0]).ToUniversalTime();
        var endDate = DateTime.Parse(splittedText[1]).ToUniversalTime();
        
        var visitedEvents = (await eventsRepository.GetEventsForPeriodAndArrangementAsync(startDate, endDate, arrangementGuid)).Where(@event =>
        {
            var presencesInEvent = presenceRepository.GetPresencesForEventAsync(@event.EventId).GetAwaiter().GetResult(); // pizdec
            var user = usersRepository.GetUserAsync(context.TelegramUserId).GetAwaiter().GetResult();
            if (presencesInEvent.FirstOrDefault(x => x.UserId == user.UserId) != null)
            {
                return true;
            }

            return false;
        }).ToArray();
        
        var costs = await Task.WhenAll(visitedEvents.Select(async x =>
        {
            var presencesCount = (await presenceRepository.GetPresencesForEventAsync(x.EventId)).Count();
            return (double)x.Cost / presencesCount;
        }));

        var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);

        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
            MessageText = Math.Round(costs.Sum(), 2).ToString(CultureInfo.InvariantCulture),
            Ikm = ikm.ToArray()
        };
            
        outputMessageQueue.AddMessage(answer);
    }
}