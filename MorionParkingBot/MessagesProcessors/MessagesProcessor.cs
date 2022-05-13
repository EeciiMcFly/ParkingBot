using MorionParkingBot.Database;
using MorionParkingBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	private readonly IUsersService _usersService;
	private readonly ParkingRequestQueue _parkingRequestQueue;
	private readonly StateService _stateService;
	private readonly ChatId _myChatId = new("340612851");


	private readonly FrameStateLogic _frameStateLogic;

	public MessagesProcessor(TelegramBotClient telegramBotClient,
		IUsersService usersService,
		ParkingRequestQueue parkingRequestQueue,
		StateService stateService, 
		FrameStateLogic frameStateLogic)
	{
		_telegramBotClient = telegramBotClient;
		_usersService = usersService;
		_parkingRequestQueue = parkingRequestQueue;
		_stateService = stateService;
		_frameStateLogic = frameStateLogic;
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

			return;
		}

		// if (user.IsAdmin)
		// {
		// 	await ProcessMessageFromAdmin(update);
		// }
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
	//
	// private async Task ProcessMessageFromAdmin(Update update)
	// {
	// 	var messageText = update.Message.Text;
	// 	if (messageText != null)
	// 	{
	// 		if (messageText.Contains("/info"))
	// 		{
	// 			var infoMessage = "payUrl|noSpot|goodPay";
	// 			await _telegramBotClient.SendTextMessageAsync(_myChatId, infoMessage);
	//
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("/queue"))
	// 		{
	// 			var lastUserInQueue = _parkingRequestQueue.GetLastUserInQueue();
	// 			if (lastUserInQueue == 0)
	// 				await _telegramBotClient.SendTextMessageAsync(_myChatId, "В очереди пусто");
	// 			else
	// 			{
	// 				await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 					$"Сейчас очередь пользователя: {lastUserInQueue}");
	// 			}
	//
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("/off"))
	// 		{
	// 			_stateService.IsActive = false;
	//
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("/on"))
	// 		{
	// 			_stateService.IsActive = true;
	//
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("payUrl"))
	// 		{
	// 			var payUrlStrings = messageText.Split("|");
	// 			var userId = Convert.ToInt64(payUrlStrings[0]);
	// 			var payUrl = payUrlStrings[2];
	// 			var user = await _usersService.GetOrCreateUserAsync(userId);
	// 			var payment = user.Payments.FirstOrDefault(data => !data.IsPayed);
	// 			payment.PayUrl = payUrl;
	// 			await _paymentsService.UpdatePaymentAsync(payment);
	// 			var infoMessage = "Ссылка на оплату сгенерирована." + Environment.NewLine +
	// 			                  $"Ваш идентифкатор: {userId}" + Environment.NewLine +
	// 			                  "Для оплаты нажните кнопку.";
	// 			var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
	// 			{
	// 				new[]
	// 				{
	// 					InlineKeyboardButton.WithUrl("Перейти для оплаты", payUrl),
	// 					InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu),
	// 				}
	// 			});
	// 			await _telegramBotClient.SendTextMessageAsync(userId, infoMessage, replyMarkup: inlineKeyboardMarkup);
	// 			await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 				$"Информация о ссылке на оплату отправлена пользователю: {userId}");
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("noSpot"))
	// 		{
	// 			var userId = _parkingRequestQueue.GetLastUserInQueue();
	// 			var newMessageText = "Не удалось найти свободное парковочное место перед Морионом";
	// 			var user = await _usersService.GetOrCreateUserAsync(userId);
	// 			var inlineKeyboardMarkup = CreateBaseInlineKeyboardMarkup(user.PayedUsages);
	// 			await _telegramBotClient.SendTextMessageAsync(userId, newMessageText,
	// 				replyMarkup: inlineKeyboardMarkup);
	// 			_parkingRequestQueue.RemoveFromQueue();
	//
	// 			await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 				$"Информация о парковке отправлена пользователю: {userId}");
	// 			return;
	// 		}
	//
	// 		if (messageText.Contains("goodPay"))
	// 		{
	// 			var goodPayStrings = messageText.Split("|");
	// 			var userId = Convert.ToInt64(goodPayStrings[0]);
	// 			var goodPayMessage = "Использования оплачены. Спасибо за платеж.";
	// 			var user = await _usersService.GetOrCreateUserAsync(userId);
	// 			user.PayedUsages += 10;
	// 			var payment = user.Payments.FirstOrDefault(data => !data.IsPayed);
	// 			if (payment == null)
	// 				return;
	// 			payment.IsPayed = true;
	// 			await _paymentsService.UpdatePaymentAsync(payment);
	// 			await _usersService.UpdateUserAsync(user);
	// 			var inlineKeyboardMarkup = CreateBaseInlineKeyboardMarkup(user.PayedUsages);
	// 			await _telegramBotClient.SendTextMessageAsync(userId, goodPayMessage,
	// 				replyMarkup: inlineKeyboardMarkup);
	//
	// 			await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 				$"Информация об успешной оплате отправлена пользователю: {userId}");
	// 		}
	// 	}
	//
	// 	if (true)
	// 	{
	// 		var userId = _parkingRequestQueue.GetLastUserInQueue();
	// 		var newMessageText = "Нашел парковочное место";
	// 		var fileInfo = await _telegramBotClient.GetFileAsync(update.Message.Photo.Last().FileId);
	// 		var fileSteam = new MemoryStream();
	// 		await _telegramBotClient.DownloadFileAsync(fileInfo.FilePath, fileSteam);
	// 		fileSteam.Position = 0;
	// 		var user = await _usersService.GetOrCreateUserAsync(userId);
	// 		var ikm = CreateBaseInlineKeyboardMarkup(user.PayedUsages);
	// 		await _telegramBotClient.SendTextMessageAsync(userId, newMessageText, replyMarkup: ikm);
	// 		await _telegramBotClient.SendPhotoAsync(userId, new InputOnlineFile(fileSteam));
	//
	// 		await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 			$"Информация о парковке отправлена пользователю: {userId}");
	// 		_parkingRequestQueue.RemoveFromQueue();
	// 	}
	// }
	//
	// private InlineKeyboardMarkup CreateBaseInlineKeyboardMarkup(long payedUsages)
	// {
	// 	var ikm = new InlineKeyboardMarkup(new[]
	// 	{
	// 		new[]
	// 		{
	// 			InlineKeyboardButton.WithCallbackData("Найди парковку", CallbackDataConstants.FindParkingQuery),
	// 			InlineKeyboardButton.WithCallbackData($@"Осталось использований: {payedUsages}",
	// 				CallbackDataConstants.InfoAboutPayment)
	// 		}
	// 	});
	//
	// 	return ikm;
	// }
}