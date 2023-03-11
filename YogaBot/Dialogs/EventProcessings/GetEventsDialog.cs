using System.Text;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs.EventProcessings;

public class GetEventsDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IEventsRepository eventsRepository;
    private readonly IKeyboardBuilder keyboardBuilder;

    public GetEventsDialog(IOutputMessageQueue outputMessageQueue,
        IEventsRepository eventsRepository,
        IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.eventsRepository = eventsRepository;
        this.keyboardBuilder = keyboardBuilder;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = Convert.ToInt64(context.CallbackData.Split('/')[1]);

        var events = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);
        var message = string.Empty;
        var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);

        var stringBuilder = new StringBuilder();
        foreach (var eventData in events)
        {
            stringBuilder.AppendLine($"*{eventData.Name}*");
            if (eventData.Cost > 0)
                stringBuilder.AppendLine($"Стоимость: *{eventData.Cost}*");
            var dataDate = eventData.Date - (DateTime.UtcNow - DateTime.Now);
            stringBuilder.AppendLine(dataDate.ToString("M") + " " + dataDate.ToString("t"));
            stringBuilder.AppendLine();
        }

        message = stringBuilder.ToString();
        if (String.IsNullOrEmpty(message))
        {
            message = "Занятий не найдено";
        }

        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = message,
            Ikm = ikm.ToArray()
        };

        outputMessageQueue.AddMessage(answer);
    }


    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;

        return context.CallbackData.Contains(CallbackDataConstants.GetEvents);
    }

    public int Priority => 10;
}