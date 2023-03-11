using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs;

public class DefaultDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IUsersRepository usersRepository;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;

    public DefaultDialog(IOutputMessageQueue outputMessageQueue, IUsersRepository usersRepository, 
        IArrangementRepository arrangementRepository, IUserArrangementRelationsRepository userArrangementRelationsRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.usersRepository = usersRepository;
        this.arrangementRepository = arrangementRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
    }

    public async void StartDialog(BotContext context)
    {
        var user = await usersRepository.GetUserAsync(context.TelegramUserId);

        if (user == null)
        {
            user = new User
            {
                TelegramUserId = context.TelegramUserId
            };
            await usersRepository.AddUserAsync(user);
        }

        var relations = await userArrangementRelationsRepository.GetRelationsForUserAsync(user.UserId);
        var arrangements = relations.Select(async x => await arrangementRepository.GetArrangementAsync(x.UserId)).Select(x => x.Result);


        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData("Мои мероприятия", CallbackDataConstants.AllActivities)}
        });
        
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageText = "Привет это Йога бот!",
            Ikm = ikm
        };
        if (context.CallbackData != null)
        {
            answer.MessageId = context.MessageId;
            answer.MessageType = MessageType.Change;
        }


        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;
        return context.CallbackData.Contains(CallbackDataConstants.Start);
    }

    public int Priority => 0;
}