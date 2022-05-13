using MorionParkingBot.Database;
using Telegram.Bot.Types;

namespace MorionParkingBot;

public class FrameStateLogic
{
	private readonly FrameStateConstructor _frameStateConstructor;

	public FrameStateLogic(FrameStateConstructor frameStateConstructor)
	{
		_frameStateConstructor = frameStateConstructor;
	}

	public List<FrameState> GetStartStateForUser(Update update, UserData user)
	{
		var isLicenseActive = user.LicenseInfo.ExpirationTime > DateTime.UtcNow;

		if (isLicenseActive)
		{
			var state = _frameStateConstructor.ConstructStartFrameForActiveLicense(update.Message.Chat.Id);
			return new List<FrameState> {state};
		}

		var states = _frameStateConstructor.ConstructStartFrameForInactiveLicense(update.Message.Chat.Id);
		return states;
	}

	public List<FrameState> GetActivateCodeStateForUser(Update update, UserData user)
	{
		var isLicenseActive = user.LicenseInfo.ExpirationTime > DateTime.UtcNow;
		if (isLicenseActive)
		{
			var countOfLicenseDay = (user.LicenseInfo.ExpirationTime - DateTime.UtcNow).Days + 1;
			var state = _frameStateConstructor.ConstructActivateCodeFrameForActiveLicense(update.CallbackQuery.From.Id,
				update.CallbackQuery.Message.MessageId, countOfLicenseDay);
			return new List<FrameState> {state};
		}
		else
		{
			var state =
				_frameStateConstructor.ConstructActivateCodeFrameForInactiveLicense(update.CallbackQuery.From.Id,
					update.CallbackQuery.Message.MessageId);

			return new List<FrameState> {state};
		}
	}

	public List<FrameState> GetMainMenuStateForUser(Update update, UserData user)
	{
		var isLicenseActive = user.LicenseInfo.ExpirationTime > DateTime.UtcNow;

		if (isLicenseActive)
		{
			var state = _frameStateConstructor.ConstructMainMenuStateForActiveLicense(update.CallbackQuery.From.Id,
				update.CallbackQuery.Message.MessageId);
			return new List<FrameState> {state};
		}

		var states = _frameStateConstructor.ConstructMainMenuStateForInactiveLicense(update.CallbackQuery.From.Id,
			update.CallbackQuery.Message.MessageId);
		return new List<FrameState> {states};
	}
}