using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs.EventProcessings;

public class GetEventsDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;

    public GetEventsDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter, 
        IEventsRepository eventsRepository, IArrangementRepository arrangementRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = new Guid(context.CallbackData.Split('/')[1]);

        var events = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);
        var message = string.Empty;

        foreach (var eventData in events)
        {
            message += "\n" + eventData.Name + " " + eventData.Cost + " " + (eventData.Date - (DateTime.UtcNow - DateTime.Now));
        }

        if (String.IsNullOrEmpty(message))
        {
            message = "Занятий не найдено";
        }
            
        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.MyActivities),
                InlineKeyboardButton.WithCallbackData("Запланировать занятие", CallbackDataConstants.CreateEvent + '/' + arrangementGuid),
                InlineKeyboardButton.WithCallbackData("Удалить занятие", CallbackDataConstants.DeleteEvent + '/' + arrangementGuid),
            },
        });
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = message,
            Ikm = ikm
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