using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Events;
using YogaBot.Storage.UserArrangementRelations;

namespace YogaBot.Dialogs.EventProcessings;

public class DeleteEventsDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IEventsRepository eventsRepository;
    private readonly IKeyboardBuilder keyboardBuilder;

    public DeleteEventsDialog(IOutputMessageQueue outputMessageQueue,
        IEventsRepository eventsRepository, IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.eventsRepository = eventsRepository;
        this.keyboardBuilder = keyboardBuilder;
    }

    public async void StartDialog(BotContext context)
    {
        var callbackData = context.CallbackData;
        var arrangementGuid = Convert.ToInt64(callbackData.Split('/')[1]);
        var events = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);

        if (!events.Any())
        {
            var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);
            var message = "Занятий не найдено";

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
            var message = "Выберите событие для удаления:";
            var ikm = new List<InlineKeyboardButton[]>();
            foreach (var eventData in events)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"{eventData.Name}");
                var dataDate = eventData.Date - (DateTime.UtcNow - DateTime.Now);
                stringBuilder.AppendLine(dataDate.ToString("M") + " " + dataDate.ToString("t"));
                ikm.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(stringBuilder.ToString(),
                        CallbackDataConstants.DeleteEventId + '/' + eventData.EventId + "/" + arrangementGuid)
                });
            }

            ikm.Add(new[] { InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities) });

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

        return context.CallbackData.Contains(CallbackDataConstants.DeleteEvent);
    }

    public int Priority => 10;
}