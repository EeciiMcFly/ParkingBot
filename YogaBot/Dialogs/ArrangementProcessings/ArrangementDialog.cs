using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.ArrangementProcessings;

public class ArrangementDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUsersRepository usersRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;

    public ArrangementDialog(IOutputMessageQueue outputMessageQueue,
        IArrangementRepository arrangementRepository,
        IUsersRepository usersRepository,
        IUserArrangementRelationsRepository userArrangementRelationsRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = Convert.ToInt64(context.CallbackData.Split('/')[1]);
        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
        var user = await usersRepository.GetUserAsync(context.TelegramUserId);
        var relationsForUser = await userArrangementRelationsRepository.GetRelationsForUserAsync(user.UserId);
        var role = relationsForUser.FirstOrDefault(e => e.ArrangementId == arrangement.ArrangementId)?.Role;

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
            CallbackDataConstants.CalculatePrice + '/' + arrangementGuid)});
        ikm.Add(new[]{InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities)});

        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = arrangement.Name,
            Ikm = ikm.ToArray()
        };

        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;
        return context.CallbackData.Contains(CallbackDataConstants.SelectArrangement);
    }

    public int Priority { get; }
}