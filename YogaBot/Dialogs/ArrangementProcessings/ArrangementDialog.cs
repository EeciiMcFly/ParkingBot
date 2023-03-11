using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;

namespace YogaBot.Dialogs.ArrangementProcessings;

public class ArrangementDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IArrangementRepository arrangementRepository;

    public ArrangementDialog(IOutputMessageQueue outputMessageQueue,
        IArrangementRepository arrangementRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.arrangementRepository = arrangementRepository;
    }

    public async void StartDialog(BotContext context)
    {
        var arrangementGuid = Convert.ToInt64(context.CallbackData.Split('/')[1]);
        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);

        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Запланировать занятие",
                    CallbackDataConstants.CreateEvent + '/' + arrangementGuid)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия",
                    CallbackDataConstants.GetEvents + '/' + arrangementGuid)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Удалить занятие",
                    CallbackDataConstants.DeleteEvent + '/' + arrangementGuid)
            },
            new[] { InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities) }
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

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;
        return context.CallbackData.Contains(CallbackDataConstants.SelectArrangement);
    }

    public int Priority { get; }
}