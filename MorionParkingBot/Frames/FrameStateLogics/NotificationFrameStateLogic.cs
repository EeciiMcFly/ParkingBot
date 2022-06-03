using MorionParkingBot.Users;

namespace MorionParkingBot.Frames;

public class NotificationFrameStateLogic
{
	private readonly FrameStateConstructor _frameStateConstructor;
	private readonly IUsersService _usersService;

	public NotificationFrameStateLogic(IUsersService usersService, 
		FrameStateConstructor frameStateConstructor)
	{
		_usersService = usersService;
		_frameStateConstructor = frameStateConstructor;
	}

	// public async Task<List<FrameState>> GetNotificationStateForUser(BotContext botContext)
	// {
	// 	var user = await _usersService.GetOrCreateUserAsync(botContext.TelegramUserId);
	// 	var isLicenseActive = user.LicenseInfos.First().ExpirationTime > DateTime.UtcNow;
	//
	// 	if (!isLicenseActive)
	// 	{
	// 		var startState = _frameStateConstructor.ConstructMainMenuStateForInactiveLicense(botContext.ChatId,
	// 			botContext.MessageId);
	// 		return new List<FrameState> {startState};
	// 	}
	// 	
	// 	
	// }
}