using MorionParkingBot.Database;
using MorionParkingBot.PromoCodes;
using MorionParkingBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	private readonly IUsersService _usersService;
	private readonly ChatId _myChatId = new("340612851");


	private readonly FrameStateLogic _frameStateLogic;
	private readonly IPromoCodeService _promoCodeService;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		IUsersService usersService,
		FrameStateLogic frameStateLogic, 
		IPromoCodeService promoCodeService)
	{
		_telegramBotClient = telegramBotClient;
		_usersService = usersService;
		_frameStateLogic = frameStateLogic;
		_promoCodeService = promoCodeService;
	}

	public async Task ProcessMessage(Update update)
	{
		UserData? user = null;
		var message = update.Message;
		if (message.From != null)
		{
			user = await _usersService.GetOrCreateUserAsync(message.From.Id);
		}

		if (message.Text == "/start")
		{
			await ProcessStartMessageAsync(update, user);
		}

		await ProcessPromoCodeMessageAsync(update, user);
	}

	private async Task ProcessStartMessageAsync(Update update, UserData user)
	{
		var states = _frameStateLogic.GetStartStateForUser(update, user);
		foreach (var currentState in states)
		{
			if (currentState.Ikm != null)
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText,
				replyMarkup: currentState.Ikm);
			else
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);
		}
	}

	private async Task ProcessPromoCodeMessageAsync(Update update, UserData user)
	{
		var codeString = update.Message.Text;
		var activationResult = await _promoCodeService.ActivatePromoCodeAsync(user, codeString);

		var states = new List<FrameState>();
		switch (activationResult)
		{
			case ActivationResult.SuccessesActivation:
				states = _frameStateLogic.GetPromoCodeActivatedStateForUser(update, user);
				break;
			case ActivationResult.AlreadyActivated:
				states = _frameStateLogic.GetPromoCodeAlreadyActivatedStateForUser(update, user);
				break;
			case ActivationResult.CodeNotExist:
				states = _frameStateLogic.GetPromoCodeNotExistStateForUser(update, user);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		foreach (var currentState in states)
		{
			if (currentState.Ikm != null)
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText,
					replyMarkup: currentState.Ikm);
			else
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);
		}
	}
}