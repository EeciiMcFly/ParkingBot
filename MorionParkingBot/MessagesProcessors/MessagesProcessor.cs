using MorionParkingBot.Frames;
using MorionParkingBot.PromoCodes;
using Telegram.Bot;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");


	private readonly FrameStateLogic _frameStateLogic;
	private readonly IPromoCodeService _promoCodeService;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		FrameStateLogic frameStateLogic, 
		IPromoCodeService promoCodeService)
	{
		_telegramBotClient = telegramBotClient;
		_frameStateLogic = frameStateLogic;
		_promoCodeService = promoCodeService;
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