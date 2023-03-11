using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
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
    private readonly IKeyboardBuilder keyboardBuilder;

    public ArrangementDialog(IOutputMessageQueue outputMessageQueue,
        IArrangementRepository arrangementRepository,
        IUsersRepository usersRepository,
        IUserArrangementRelationsRepository userArrangementRelationsRepository, IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
        this.keyboardBuilder = keyboardBuilder;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = Convert.ToInt64(context.CallbackData.Split('/')[1]);
        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
        var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementGuid);


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