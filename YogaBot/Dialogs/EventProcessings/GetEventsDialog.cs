using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Events;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.EventProcessings;

public class GetEventsDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IEventsRepository eventsRepository;
    private readonly IUsersRepository usersRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;

    public GetEventsDialog(IOutputMessageQueue outputMessageQueue,
        IEventsRepository eventsRepository,
        IUsersRepository usersRepository,
        IUserArrangementRelationsRepository userArrangementRelationsRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.eventsRepository = eventsRepository;
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = Convert.ToInt64(context.CallbackData.Split('/')[1]);

        var events = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);
        var message = string.Empty;
        var user = await usersRepository.GetUserAsync(context.TelegramUserId);
        var relationsForUser = await userArrangementRelationsRepository.GetRelationsForUserAsync(user.UserId);
        var role = relationsForUser.FirstOrDefault(e => e.ArrangementId == arrangementGuid)?.Role;

        var ikm = new List<InlineKeyboardButton[]>();
        ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия",
            CallbackDataConstants.GetEvents + '/' + arrangementGuid)});
        if (role == Role.Admin || role == Role.Trainer)
        {
            ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Запланировать занятие",
                CallbackDataConstants.CreateEvent + '/' + arrangementGuid)});
            ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Удалить занятие",
                CallbackDataConstants.DeleteEvent + '/' + arrangementGuid)});
        }
        ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Сколько я должен?", 
            CallbackDataConstants.CalculatePriceForRequester + '/' + arrangementGuid)});
        ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities)});

        foreach (var eventData in events)
        {
            message += "\n" + eventData.Name + " " + eventData.Cost + " " + (eventData.Date - (DateTime.UtcNow - DateTime.Now));
        }

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