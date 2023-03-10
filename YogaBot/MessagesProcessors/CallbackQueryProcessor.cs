using YogaBot.DialogEngine;
using Telegram.Bot;

namespace YogaBot.MessagesProcessors;

public class CallbackQueryProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	private readonly IDialogStateStorage dialogStateStorage;
	private readonly IDialogSelector dialogSelector;
	//private readonly ChatId _myChatId = new("340612851");
	

	public CallbackQueryProcessor(TelegramBotClient telegramBotClient, IDialogStateStorage dialogStateStorage,
		IDialogSelector dialogSelector)
	{
		_telegramBotClient = telegramBotClient;
		this.dialogStateStorage = dialogStateStorage;
		this.dialogSelector = dialogSelector;
	}
	
	public async Task ProcessCallbackQuery(BotContext botContext)
	{
		var userAction = dialogStateStorage.GetUserState(botContext.TelegramUserId);
		if (userAction != null)
		{
			userAction(botContext);
			return;
		}

		var dialog = dialogSelector.SelectDialog(botContext);
		dialog.StartDialog(botContext);
	}
}