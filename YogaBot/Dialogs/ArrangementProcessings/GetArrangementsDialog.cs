using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs.ArrangementProcessings;

public class GetArrangementsDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;
    private readonly IUsersRepository usersRepository;

    public GetArrangementsDialog(IOutputMessageQueue outputMessageQueue,
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
        var innerUserId = await usersRepository.GetUserAsync(context.TelegramUserId)
                          ?? throw new Exception("You do not exist");

        var relations = await userArrangementRelationsRepository.GetRelationsForUserAsync(innerUserId.UserId);
        var arrangements =
            await Task.WhenAll(relations.Select(x => arrangementRepository.GetArrangementAsync(x.ArrangementId)));

        var message = "";
        if (!arrangements.Any())
        {
            message = @"Сейчас у вас нет действующих мероприятий.
Чтобы начать работу - создай группу для мероприятия.
Сначала добавь в неё меня в качестве администратора, а уже затем участников!";
        }
        else
        {
            message = "Выберете мероприятие:";
        }

        var ikm = arrangements.Select(x =>
                InlineKeyboardButton.WithCallbackData(x.Name,
                    CallbackDataConstants.SelectArrangement + "/" + x.ArrangementId))
            .ToList();
        ikm.Add(InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.Start));

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

        return context.CallbackData.Equals(CallbackDataConstants.AllActivities);
    }

    public int Priority => 10;
}