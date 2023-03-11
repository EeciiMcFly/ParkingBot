using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs.EventProcessings;

public class NewDeleteEvent : IDialog<BotContext>
{
    private readonly IEventsRepository eventsRepository;
    private readonly IKeyboardBuilder keyboardBuilder;
    private readonly IOutputMessageQueue outputMessageQueue;

    public NewDeleteEvent(IEventsRepository eventsRepository, IKeyboardBuilder keyboardBuilder, IOutputMessageQueue outputMessageQueue)
    {
        this.eventsRepository = eventsRepository;
        this.keyboardBuilder = keyboardBuilder;
        this.outputMessageQueue = outputMessageQueue;
    }

    public async void StartDialog(BotContext context)
    {
        var callbackData = context.CallbackData;
        var eventId = Convert.ToInt64(callbackData.Split('/')[1]);
        var arrangementGuid = Convert.ToInt64(callbackData.Split('/')[2]);
        var @event = await eventsRepository.GetEventAsync(eventId);

        if (@event == null)
        {
            var message = "Не найдено событие";
            var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);

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
        else
        {
            await eventsRepository.DeleteEventAsync(@event.EventId);

            var message = "Событие успешно удалено";
            var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);

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
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;

        return context.CallbackData.Contains(CallbackDataConstants.DeleteEventId);
    }

    public int Priority => 10;
}