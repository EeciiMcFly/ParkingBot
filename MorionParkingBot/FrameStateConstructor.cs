using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot;

public class FrameStateConstructor
{
	public FrameState ConstructStartFrameForActiveLicense(long chatId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Найди парковку", CallbackDataConstants.FindParkingQuery),
				InlineKeyboardButton.WithCallbackData("Ввести промокод", CallbackDataConstants.ActivateCode)
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

	public List<FrameState> ConstructStartFrameForInactiveLicense(long chatId)
	{
		var firstFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.StartMessage,
		};

		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Ввести промокод", CallbackDataConstants.ActivateCode)
			}
		});

		var secondFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.InactiveLicenseStartMessage,
			Ikm = ikm
		};

		var frameStateList = new List<FrameState>
		{
			firstFrameState,
			secondFrameState
		};

		return frameStateList;
	}
}