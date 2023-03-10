using Telegram.Bot;
using ILogger = Serilog.ILogger;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");

	private readonly ILogger _logger;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		ILogger logger)
	{
		_telegramBotClient = telegramBotClient;
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
	}

	private async Task ProcessPromoCodeMessageAsync(BotContext botContext)
	{
		_logger.Information($"Process promocode data for user - {botContext.TelegramUserId}");
	}
}