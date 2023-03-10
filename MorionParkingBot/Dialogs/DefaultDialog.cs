using MorionParkingBot.Constants;
using MorionParkingBot.Frames;
using MorionParkingBot.MessageQueue;
using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot.Dialogs;

public class DefaultDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;

    public DefaultDialog(IOutputMessageQueue outputMessageQueue)
    {
        this.outputMessageQueue = outputMessageQueue;
    }


    public void StartDialog(BotContext context)
    {
        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Мои мероприятия", CallbackDataConstants.MyActivities),
            },
        });
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageText = "Привет это Йога бот!",
            Ikm = ikm
        };

        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        return true;
    }

    public int Priority => 0;
}