using MorionParkingBot.Constants;
using MorionParkingBot.Frames;
using SixLabors.ImageSharp.Formats.Jpeg;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace MorionParkingBot.MessagesProcessors;

public class CallbackQueryProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	//private readonly ChatId _myChatId = new("340612851");
	
	private readonly FrameStateLogic _frameStateLogic;

	public CallbackQueryProcessor(TelegramBotClient telegramBotClient,
		FrameStateLogic frameStateLogic)
	{
		_telegramBotClient = telegramBotClient;
		_frameStateLogic = frameStateLogic;
	}
	
	public async Task ProcessCallbackQuery(BotContext botContext)
	{
		switch (botContext.CallbackData)
		{
			case CallbackDataConstants.ActivateCode:
				await ProcessActivateCode(botContext);
				break;
			
			case CallbackDataConstants.BackToMainMenu:
				await ProcessBackToMainMenu(botContext);
				break;
			
			case CallbackDataConstants.FindParkingQuery:
				await ProcessFindParking(botContext);
				break;
			
			default:
				if (botContext.CallbackData.Contains("parkingId"))
					await ProcessParkingId(botContext);
				if (botContext.CallbackData.Contains("next"))
					await ProcessNextParkings(botContext);
				break;
		}
	}

	private async Task ProcessActivateCode(BotContext botContext)
	{
		var states = await _frameStateLogic.GetActivateCodeStateForUser(botContext);
		foreach (var currentState in states)
		{
			await _telegramBotClient.EditMessageTextAsync(currentState.ChatId,
			 		currentState.MessageId, currentState.MessageText, replyMarkup: currentState.Ikm);
		}
	}

	private async Task ProcessBackToMainMenu(BotContext botContext)
	{
		var states = await _frameStateLogic.GetMainMenuStateForUser(botContext);
		foreach (var currentState in states)
		{
			await _telegramBotClient.EditMessageTextAsync(currentState.ChatId,
				currentState.MessageId, currentState.MessageText, replyMarkup: currentState.Ikm);
		}
	}
	
	private async Task ProcessFindParking(BotContext botContext)
	{
		var states = await _frameStateLogic.GetFingParkingStateForUser(botContext);
		foreach (var currentState in states)
		{
			await _telegramBotClient.EditMessageTextAsync(currentState.ChatId,
				currentState.MessageId, currentState.MessageText, replyMarkup: currentState.Ikm);
		}
	}

	private async Task ProcessNextParkings(BotContext botContext)
	{
		var states = await _frameStateLogic.GetNextParkingsForUser(botContext);
		foreach (var currentState in states)
		{
			await _telegramBotClient.EditMessageTextAsync(currentState.ChatId,
				currentState.MessageId, currentState.MessageText, replyMarkup: currentState.Ikm);
		}
	}

	private async Task ProcessParkingId(BotContext botContext)
	{
		var states = await _frameStateLogic.GetParkingStateForUser(botContext);
		foreach (var currentState in states)
		{
			if (currentState.Images != null)
			{
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);
				foreach (var image in currentState.Images)
				{
					using (var ms = new MemoryStream())
					{
						var imageEncoder = new JpegEncoder();
						image.Save(ms, imageEncoder);
						ms.Position = 0;
						await _telegramBotClient.SendPhotoAsync(currentState.ChatId, new InputOnlineFile(ms));
					}
				}
				
				continue;
			}
	
			if (currentState.Ikm != null)
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText,
					replyMarkup: currentState.Ikm);
			else
				await _telegramBotClient.SendTextMessageAsync(currentState.ChatId, currentState.MessageText);
		}
	}
}