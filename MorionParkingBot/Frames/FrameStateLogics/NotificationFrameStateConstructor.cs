using MorionParkingBot.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot.Frames;

public class NotificationFrameStateConstructor
{
	public FrameState ConstructNotificationFrame(long chatId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Расписание", CallbackDataConstants.FindParkingQuery),
				InlineKeyboardButton.WithCallbackData("Ввести промокод", CallbackDataConstants.ActivateCode)
			},
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Уведомления", CallbackDataConstants.NotificationQuery), 
			}
		});

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.StartMessage,
			Ikm = ikm
		};

		return frameState;
	}
}