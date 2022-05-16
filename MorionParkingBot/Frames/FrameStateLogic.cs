using MorionParkingBot.Users;
using Telegram.Bot.Types;

namespace MorionParkingBot.Frames;

public class FrameStateLogic
{
	private readonly FrameStateConstructor _frameStateConstructor;
	private readonly IUsersService _usersService;

	public FrameStateLogic(FrameStateConstructor frameStateConstructor, 
		IUsersService usersService)
	{
		_frameStateConstructor = frameStateConstructor;
		_usersService = usersService;
	}

	public async Task<List<FrameState>> GetStartStateForUser(BotContext botContext)
	{
		var user = await _usersService.GetOrCreateUserAsync(botContext.TelegramUserId);
		var isLicenseActive = user.LicenseInfos.First().ExpirationTime > DateTime.UtcNow;

		if (isLicenseActive)
		{
			var state = _frameStateConstructor.ConstructStartFrameForActiveLicense(botContext.ChatId);
			return new List<FrameState> {state};
		}

		var states = _frameStateConstructor.ConstructStartFrameForInactiveLicense(botContext.ChatId);
		return states;
	}

	public async Task<List<FrameState>> GetActivateCodeStateForUser(BotContext botContext)
	{
		var user = await _usersService.GetOrCreateUserAsync(botContext.TelegramUserId);
		var isLicenseActive = user.LicenseInfos.First().ExpirationTime > DateTime.UtcNow;
		if (isLicenseActive)
		{
			var countOfLicenseDay = (user.LicenseInfos.First().ExpirationTime - DateTime.UtcNow).Days + 1;
			var state = _frameStateConstructor.ConstructActivateCodeFrameForActiveLicense(botContext.ChatId,
				botContext.MessageId, countOfLicenseDay);
			return new List<FrameState> {state};
		}
		else
		{
			var state =
				_frameStateConstructor.ConstructActivateCodeFrameForInactiveLicense(botContext.ChatId,
					botContext.MessageId);

			return new List<FrameState> {state};
		}
	}

	public async Task<List<FrameState>> GetMainMenuStateForUser(BotContext botContext)
	{
		var user = await _usersService.GetOrCreateUserAsync(botContext.TelegramUserId);
		var isLicenseActive = user.LicenseInfos.First().ExpirationTime > DateTime.UtcNow;

		if (isLicenseActive)
		{
			var state = _frameStateConstructor.ConstructMainMenuStateForActiveLicense(botContext.ChatId,
				botContext.MessageId);
			return new List<FrameState> {state};
		}

		var states = _frameStateConstructor.ConstructMainMenuStateForInactiveLicense(botContext.ChatId,
			botContext.MessageId);
		return new List<FrameState> {states};
	}

	public List<FrameState> GetPromoCodeNotExistStateForUser(BotContext botContext)
	{
		var state = _frameStateConstructor.ConstructPromoCodeNotExistFrame(botContext.ChatId);
		return new List<FrameState> {state};
	}

	public List<FrameState> GetPromoCodeAlreadyActivatedStateForUser(BotContext botContext)
	{
		var state = _frameStateConstructor.ConstructPromoCodeAlreadyActivatedFrame(botContext.ChatId);
		return new List<FrameState> {state};
	}

	public List<FrameState> GetPromoCodeActivatedStateForUser(BotContext botContext)
	{
		var state = _frameStateConstructor.ConstructPromoCodeActivatedFrame(botContext.ChatId);
		return state;
	}

	public List<FrameState> GetFingParkingStateForUser(Update update, UserData userData)
	{
		var state = _frameStateConstructor.ConstructPromoCodeActivatedFrame(update.Message.Chat.Id);
		return state;
	}
}