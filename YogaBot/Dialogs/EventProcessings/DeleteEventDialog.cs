using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.EventProcessings;

public class DeleteEventDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUsersRepository usersRepository;

    public DeleteEventDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter, 
        IEventsRepository eventsRepository, IArrangementRepository arrangementRepository, IUsersRepository usersRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
    }

    public void StartDialog(BotContext context)
    {
        var message = "Введите дату отменяемого занятия";
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
            
        return context.CallbackData.Contains(CallbackDataConstants.DeleteEvent);
    }

    public int Priority => 10;

    private async void ProcessArrangementData(BotContext context, string callbackData)
    {
        var date = DateTime.Parse(context.MessageText).ToUniversalTime();
        var arrangementGuid = Convert.ToInt64(callbackData.Split('/')[1]);
        var events = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);
        
        await eventsRepository.DeleteEventAsync(events.FirstOrDefault(x => x.Date == date).EventId);

        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
        
        var events1 = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);
        var grtg = events1.Where(x => x.Date == date).ToList();
            
        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.MyActivities),
                InlineKeyboardButton.WithCallbackData("Запланировать занятие", CallbackDataConstants.CreateEvent + '/' + arrangementGuid),
                InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия", CallbackDataConstants.GetEvents + '/' + arrangementGuid),
            },
        });
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = arrangement.Name,
            Ikm = ikm
        };
            
        outputMessageQueue.AddMessage(answer);
    }
}