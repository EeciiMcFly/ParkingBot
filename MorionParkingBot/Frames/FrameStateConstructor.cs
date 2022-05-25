using MorionParkingBot.Constants;
using MorionParkingBot.Parkings;
using SixLabors.ImageSharp;
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

	public FrameState ConstructSingleFindParkingFrame(long chatId, int messageId, List<ParkingData> parkingDatas)
	{
		var buttonsArray = new InlineKeyboardButton[parkingDatas.Count + 1];
		for (int i = 0; i < parkingDatas.Count; i++)
		{
			var parkingIdQueryData = $"parkingId:{parkingDatas[i].Id}";
			buttonsArray[i] = InlineKeyboardButton.WithCallbackData($"{parkingDatas[i].Name}", parkingIdQueryData);
		}

		buttonsArray[parkingDatas.Count] =
			InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu);

		var finalList = new List<List<InlineKeyboardButton>>();
		for (int i = 0; i < buttonsArray.Length; i++)
		{
			if (i % 3 == 0)
			{
				finalList.Add(new List<InlineKeyboardButton>());
			}
			finalList.Last().Add(buttonsArray[i]);
		}

		var ikm = new InlineKeyboardMarkup(finalList);

		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.ChooseParking,
			MessageId = messageId,
			Ikm = ikm
		};

		return frameState;
	}
	
	public FrameState ConstructPluralFindParkingFrame(long chatId, int messageId, List<ParkingData> parkingDatas)
	{
		var buttonsArray = new InlineKeyboardButton[6];
		for (int i = 0; i < 4; i++)
		{
			var parkingIdQueryData = $"parkingId:{parkingDatas[i].Id}";
			buttonsArray[i] = InlineKeyboardButton.WithCallbackData($"{parkingDatas[i].Name}", parkingIdQueryData);
		}
	
		buttonsArray[4] =
			InlineKeyboardButton.WithCallbackData("В меню", CallbackDataConstants.BackToMainMenu);
		var nextPortionQuery = $"next:{2}";
		buttonsArray[5] =
			InlineKeyboardButton.WithCallbackData("Далее", nextPortionQuery);
	
		var finalList = new List<List<InlineKeyboardButton>>();
		for (int i = 0; i < buttonsArray.Length; i++)
		{
			if (i % 3 == 0)
			{
				finalList.Add(new List<InlineKeyboardButton>());
			}
			finalList.Last().Add(buttonsArray[i]);
		}
		
		var ikm = new InlineKeyboardMarkup(finalList);
	
		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.ChooseParking,
			MessageId = messageId,
			Ikm = ikm
		};
	
		return frameState;
	}

	public FrameState ConstructNextPortionFrame(long chatId, int messageId, int portionNumber,
		List<ParkingData> parkingDatas)
	{
		var count = 0;
		var isExistNextPortion = parkingDatas.Count - 4 * (portionNumber)  > 0;
		var portionSize = Math.Min(4, parkingDatas.Count - 4 * (portionNumber - 1));
		var fullSize = portionSize;
		if (isExistNextPortion)
			fullSize += 2;
		else
			fullSize += 1;

		var buttonsArray = new InlineKeyboardButton[fullSize];
		for (int i = (portionNumber - 1) * 4; count != portionSize; i++)
		{
			var parkingIdQueryData = $"parkingId:{parkingDatas[i].Id}";
			buttonsArray[count] = InlineKeyboardButton.WithCallbackData($"{parkingDatas[i].Name}", parkingIdQueryData);
			count++;
		}
	
		buttonsArray[count++] =
			InlineKeyboardButton.WithCallbackData("В меню", CallbackDataConstants.BackToMainMenu);
		if (isExistNextPortion)
		{
			var nextPortionQuery = $"next:{portionNumber + 1}";
			buttonsArray[count] =
				InlineKeyboardButton.WithCallbackData("Далее", nextPortionQuery);
		}

		var finalList = new List<List<InlineKeyboardButton>>();
		for (int i = 0; i < buttonsArray.Length; i++)
		{
			if (i % 3 == 0)
			{
				finalList.Add(new List<InlineKeyboardButton>());
			}
			finalList.Last().Add(buttonsArray[i]);
		}
		
		var ikm = new InlineKeyboardMarkup(finalList);
	
		var frameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.ChooseParking,
			MessageId = messageId,
			Ikm = ikm
		};
	
		return frameState;
	}

	public List<FrameState> ConstructNoParkingFrame(long chatId, string parkingName)
	{
		var messageText = string.Format(MessageConstants.NoParking, parkingName);
		var firstFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = messageText,
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

		return new List<FrameState> {firstFrameState, secondFrameState};
	}

	public List<FrameState> ConstructFoundParkingFrame(long chatId, List<Image> images)
	{
		var firstFrameState = new FrameState
		{
			ChatId = chatId,
			MessageText = MessageConstants.ParkingFound,
			Images = images
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

		return new List<FrameState> {firstFrameState, secondFrameState};
	}
}