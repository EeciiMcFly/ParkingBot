using MorionParkingBot.Frames;
using MorionParkingBot.PromoCodes;
using Telegram.Bot;
using ILogger = Serilog.ILogger;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");


	private readonly FrameStateLogic _frameStateLogic;
	private readonly IPromoCodeService _promoCodeService;
	private readonly ILogger _logger;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		FrameStateLogic frameStateLogic, 
		IPromoCodeService promoCodeService, 
		ILogger logger)
	{
		_telegramBotClient = telegramBotClient;
		_frameStateLogic = frameStateLogic;
		_promoCodeService = promoCodeService;
		_logger = logger;
	}

	public async Task ProcessMessage(BotContext botContext)
	{
		if (botContext.MessageText == "/start")
		{
			await ProcessStartMessageAsync(botContext);
			
			return;
		}

		await ProcessPromoCodeMessageAsync(botContext);
	}

	private async Task ProcessStartMessageAsync(BotContext botContext)
	{
		_logger.Information($"Process start command for user - {botContext.TelegramUserId}");
		var states = await _frameStateLogic.GetStartStateForUser(botContext);
		foreach (var currentState in states)
		{
			if (currentState.Ikm != null)
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText,
				replyMarkup: currentState.Ikm);
			else
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);
		}
	}

	private async Task ProcessPromoCodeMessageAsync(BotContext botContext)
	{
		_logger.Information($"Process promocode data for user - {botContext.TelegramUserId}");
		var activationResult = await _promoCodeService.ActivatePromoCodeAsync(botContext.MessageText, botContext);

		var states = new List<FrameState>();
		switch (activationResult)
		{
			case ActivationResult.SuccessesActivation:
				states = _frameStateLogic.GetPromoCodeActivatedStateForUser(botContext);
				break;
			case ActivationResult.AlreadyActivated:
				states = _frameStateLogic.GetPromoCodeAlreadyActivatedStateForUser(botContext);
				break;
			case ActivationResult.CodeNotExist:
				states = _frameStateLogic.GetPromoCodeNotExistStateForUser(botContext);
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