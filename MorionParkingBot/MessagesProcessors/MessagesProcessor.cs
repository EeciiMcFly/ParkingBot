using MorionParkingBot.Frames;
using MorionParkingBot.MessageQueue;
using Telegram.Bot;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");
	
	private readonly IInputMessageQueue inputMessageQueue;
	private readonly IOutputMessageQueue outputMessageQueue;

	private CancellationTokenSource cancellationTokenSource;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		IInputMessageQueue inputMessageQueue,
		IOutputMessageQueue outputMessageQueue)
	{
		_telegramBotClient = telegramBotClient;
		this.inputMessageQueue = inputMessageQueue;
		this.outputMessageQueue = outputMessageQueue;
	}

	public async Task ProcessMessage(BotContext botContext)
	{
		if (botContext.MessageText == "/start")
		{
			await ProcessStartMessageAsync(botContext);
			
			return;
		}

		//await ProcessPromoCodeMessageAsync(botContext);
	}

	private async Task ProcessStartMessageAsync(BotContext botContext)
	{
		var firstFrameState = new FrameState
		{
			ChatId = botContext.ChatId,
			MessageText = "Привет это Йога бот!",
		};

		var frameStateList = new List<FrameState>
		{
			firstFrameState
		};

		foreach (var currentState in frameStateList)
		{
			outputMessageQueue.AddMessage(currentState);
			/*if (currentState.Ikm != null)
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText,
				replyMarkup: currentState.Ikm);
			else
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);*/
		}
	}

	/*private async Task ProcessPromoCodeMessageAsync(BotContext botContext)
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
	}*/
}