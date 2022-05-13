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

	public async Task<List<FrameState>> GetStartStateForUser(Update update, UserData user)
	{
		var isLicenseActive = user.LicenseInfo.ExpirationTime > DateTime.Now;

		if (isLicenseActive)
		{
			var state = _frameStateConstructor.ConstructStartFrameForActiveLicense(update.Message.Chat.Id);
			return new List<FrameState> {state};
		}

		var states = _frameStateConstructor.ConstructStartFrameForInactiveLicense(update.Message.Chat.Id);
		return states;
	}
}