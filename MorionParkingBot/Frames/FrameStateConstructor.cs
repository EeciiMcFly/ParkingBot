using MorionParkingBot.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot.Frames;

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

	public FrameState ConstructActivateCodeFrameForActiveLicense(long chatId, int messageId, int countOfLicenseDay)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu)
			}
		});

		var messageText = string.Format(MessageConstants.ActiveLicenseInfoUseCode, countOfLicenseDay);

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = messageText,
			Ikm = ikm,
			MessageId = messageId
		};

		return frameState;
	}

	public FrameState ConstructActivateCodeFrameForInactiveLicense(long chatId, int messageId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu)
			}
		});

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.InactiveLicenseUseCode,
			Ikm = ikm,
			MessageId = messageId
		};

		return frameState;
	}

	public FrameState ConstructMainMenuStateForActiveLicense(long chatId, int messageId)
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
			Ikm = ikm,
			MessageId = messageId
		};

		return frameState;
	}

	public FrameState ConstructMainMenuStateForInactiveLicense(long chatId, int messageId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Ввести промокод", CallbackDataConstants.ActivateCode)
			}
		});

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.InactiveLicenseStartMessage,
			Ikm = ikm,
			MessageId = messageId
		};

		return frameState;
	}

	public FrameState ConstructPromoCodeNotExistFrame(long chatId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu)
			}
		});

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.PromoCodeNotExist,
			Ikm = ikm
		};

		return frameState;
	}

	public FrameState ConstructPromoCodeAlreadyActivatedFrame(long chatId)
	{
		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu)
			}
		});

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.PromoCodeAlreadyActivated,
			Ikm = ikm
		};

		return frameState;
	}

	public List<FrameState> ConstructPromoCodeActivatedFrame(long chatId)
	{
		var firstFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.PromoCodeActivated,
		};

		var ikm = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				InlineKeyboardButton.WithCallbackData("Найди парковку", CallbackDataConstants.FindParkingQuery),
				InlineKeyboardButton.WithCallbackData("Ввести промокод", CallbackDataConstants.ActivateCode)
			}
		});

		var secondFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.StartMessage,
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