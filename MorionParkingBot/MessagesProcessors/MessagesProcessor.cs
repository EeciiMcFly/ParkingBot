using MorionParkingBot.DialogEngine;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	//private readonly ChatId _myChatId = new("340612851");

	private readonly IDialogStateStorage dialogStateStorage;
	private readonly IDialogSelector dialogSelector;

	public MessagesProcessor(IDialogStateStorage dialogStateStorage, IDialogSelector dialogSelector)
	{
		this.dialogStateStorage = dialogStateStorage;
		this.dialogSelector = dialogSelector;
	}

	public async Task ProcessMessage(BotContext botContext)
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