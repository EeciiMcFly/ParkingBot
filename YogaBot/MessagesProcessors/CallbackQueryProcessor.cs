using Telegram.Bot;

namespace YogaBot.MessagesProcessors;

public class CallbackQueryProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");
	

	public CallbackQueryProcessor(TelegramBotClient telegramBotClient)
	{
		_telegramBotClient = telegramBotClient;
	}
	
	public async Task ProcessCallbackQuery(BotContext botContext)
	{
		
	}
}