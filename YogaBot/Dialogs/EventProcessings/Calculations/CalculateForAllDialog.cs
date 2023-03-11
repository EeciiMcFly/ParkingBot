using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.InfoProvider;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;


namespace YogaBot.Dialogs.EventProcessings.Calculations;

public class CalculateForAllDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IPresenceRepository presenceRepository;
    private readonly IInfoProvider infoProvider;
    private readonly IKeyboardBuilder keyboardBuilder;

    public CalculateForAllDialog(IOutputMessageQueue outputMessageQueue,
        IDialogStateSetter dialogStateSetter,
        IEventsRepository eventsRepository,
        IArrangementRepository arrangementRepository,
        IPresenceRepository presenceRepository,
        IInfoProvider infoProvider,
        IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
        this.presenceRepository = presenceRepository;
        this.infoProvider = infoProvider;
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

        return context.CallbackData.Contains(CallbackDataConstants.CalculatePriceForAll);
    }

    public int Priority => 10;

    private async void ProcessArrangementData(BotContext context, string callbackData)
    {
        var arrangementGuid = Int64.Parse(callbackData.Split('/')[1]);
        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
        var splittedText = context.MessageText.Split("\n");

        var startDate = DateTime.Parse(splittedText[0]).ToUniversalTime();
        var endDate = DateTime.Parse(splittedText[1]).ToUniversalTime();

        var events = (await eventsRepository.GetEventsForPeriodAndArrangementAsync(startDate, endDate, arrangementGuid))
            .ToArray();
        var presencesGroupedByEvent = new List<IEnumerable<Presence>>();
        foreach (var @event in events)
        {
            var presencesForEvent = await presenceRepository.GetPresencesForEventAsync(@event.EventId);
            presencesGroupedByEvent.Add(presencesForEvent);
        }

        var visitors = presencesGroupedByEvent.SelectMany(x => x)
            .Select(x => x.User)
            .GroupBy(x => x.UserId)
            .Select(x => x.First());

        var costsPerUsers = new List<CostPerUser>();
        foreach (var visitor in visitors)
        {
            int sum = 0;
            foreach (var @event in events)
            {
                var presences = await presenceRepository.GetPresencesForEventAsync(@event.EventId);
                if (presences.Any(e => e.UserId == visitor.UserId))
                {
                    var count = presences.Count();
                    if (count != 0)
                    {
                        sum += @event.Cost / presences.Count();
                    }
                }
            }

            var costPerUser = new CostPerUser
            {
                cost = sum,
                telegramUsername = (await infoProvider.GetChatMember(arrangement.ChatId, visitor.TelegramUserId)).User
                    .Username
            };
            costsPerUsers.Add(costPerUser);
        }

        var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);

        var resultStrings = costsPerUsers.Select(x => "@" + x.telegramUsername + " " + Math.Round(x.cost, 2));
        var message = "";

        foreach (var resultString in resultStrings)
        {
            message += "\n" + resultString;
        }

        if (string.IsNullOrEmpty(message))
        {
            message = "В заданный период не было платных событий";
        }

        var resultMessage = new FrameState
        {
            MessageText = message,
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
        };

        outputMessageQueue.AddMessage(resultMessage);

        var menuMessage = new FrameState
        {
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
            MessageText = arrangement.Name,
            Ikm = ikm.ToArray()
        };

        outputMessageQueue.AddMessage(menuMessage);
    }

    private class CostPerUser
    {
        public double cost;
        public string telegramUsername;
    }
}