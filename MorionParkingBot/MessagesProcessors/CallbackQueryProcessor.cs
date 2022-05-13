﻿using System.Text;
using MorionParkingBot.Database;
using MorionParkingBot.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot.MessagesProcessors;

public class CallbackQueryProcessor
{
	private readonly TelegramBotClient _telegramBotClient;
	private readonly IUsersService _usersService;
	private readonly ParkingRequestQueue _parkingRequestQueue;
	private readonly StateService _stateService;
	private readonly ChatId _myChatId = new("340612851");

	public CallbackQueryProcessor(TelegramBotClient telegramBotClient, 
		IUsersService usersService,
		ParkingRequestQueue parkingRequestQueue, 
		StateService stateService)
	{
		_telegramBotClient = telegramBotClient;
		_usersService = usersService;
		_parkingRequestQueue = parkingRequestQueue;
		_stateService = stateService;
	}
	
	// public async Task ProcessCallbackQuery(Update update)
	// {
	// 	var user = await _usersService.GetOrCreateUserAsync(update.CallbackQuery.From.Id);
	//
	// 	if (!_stateService.IsActive)
	// 	{
	// 		var paymentMessage = "К сожалению бот сейчас не активен";
	// 		await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.From.Id, paymentMessage);
	// 		
	// 		return;
	// 	}
	// 	
	// 	switch (update.CallbackQuery.Data)
	// 	{
	// 		case CallbackDataConstants.FindParkingQuery:
	// 			await ProcessFindParkingAsync(update, user);
	// 			break;
	//
	// 		case CallbackDataConstants.InfoAboutPayment:
	// 			await ProcessInfoAboutPayment(update, user);
	//
	// 			break;
	//
	// 		case CallbackDataConstants.GeneratePaymentRequest:
	// 			await ProcessGeneratePaymentAsync(update, user);
	// 			break;
	//
	// 		case CallbackDataConstants.BackToMainMenu:
	// 			await ProcessBackToMainMenu(update, user);
	// 			break;
	//
	// 		default:
	// 			break;
	// 	}
	// }
	//
	// private async Task ProcessBackToMainMenu(Update update, UserData user, string text = "Нажми кнопку и бот найдет парковочное место")
	// {
	// 	var ikm = new InlineKeyboardMarkup(new[]
	// 	{
	// 		new[]
	// 		{
	// 			InlineKeyboardButton.WithCallbackData("Найди парковку", CallbackDataConstants.FindParkingQuery),
	// 			InlineKeyboardButton.WithCallbackData($@"Осталось использований: {user.PayedUsages}",
	// 				CallbackDataConstants.InfoAboutPayment)
	// 		}
	// 	});
	//
	// 	await _telegramBotClient.EditMessageTextAsync(update.CallbackQuery.From.Id,
	// 		update.CallbackQuery.Message.MessageId, text, replyMarkup: ikm);
	// }
	//
	// private async Task ProcessGeneratePaymentAsync(Update update, UserData user)
	// {
	// 	var paymentMessage = "В течении некоторого времени будет сформирована ссылка на оплату";
	// 	await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.From.Id, paymentMessage);
	//
	// 	var paymentRequestForAdmin = $"Пользователь {update.CallbackQuery.From.Id} запросил ссылку на оплату";
	// 	await _telegramBotClient.SendTextMessageAsync(_myChatId, paymentRequestForAdmin);
	//
	// 	await _paymentsService.CreateNewPaymentAsync(user);
	//
	// 	await ProcessBackToMainMenu(update, user);
	// }
	//
	// private async Task ProcessFindParkingAsync(Update update,UserData user)
	// {
	// 	var tryAdd = _parkingRequestQueue.AddInQueue(update.CallbackQuery.From.Id);
	// 	if (!tryAdd)
	// 	{
	// 		var alreadyWait = "Уже ищу, пожалуйста подождите.";
	// 		await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.From.Id, alreadyWait);
	// 		return;
	// 	}
	//
	// 	user.PayedUsages -= 1;
	// 	user.CountOfUsage += 1;
	// 	await _usersService.UpdateUserAsync(user);
	// 	await _telegramBotClient.SendTextMessageAsync(_myChatId,
	// 		$"Сообщение от пользователя {update.CallbackQuery.From.Id}: Найди парковку");
	// 			
	// 	var waitFindMessage = "Начал поиск парковочного места. Пожалуйста, подождите.";
	// 	await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.From.Id, waitFindMessage);
	// }
	//
	// private async Task ProcessInfoAboutPayment(Update update, UserData user)
	// {
	// 	var stringBuilder = new StringBuilder();
	// 	stringBuilder.AppendLine($"Количество оставшихся использований: {user.PayedUsages}.");
	// 	if (user.Payments != null)
	// 	{
	// 		var noPayedPayment = user.Payments.FirstOrDefault(data => !data.IsPayed);
	// 		if (noPayedPayment != null)
	// 		{
	// 			var payUrl = noPayedPayment.PayUrl;
	// 			var isPayUrlAdded = !string.IsNullOrEmpty(payUrl);
	// 			if (isPayUrlAdded)
	// 			{
	// 				var notPayedRequest = "Запрос на оплату уже сформирован." +
	// 				                      Environment.NewLine +
	// 				                      "Если вы его уже оплатили, то использования начисляться через некоторое время." +
	// 				                      Environment.NewLine +
	// 				                      "Если вы его не оплатили, то его необходимо оплатить.";
	// 				stringBuilder.AppendLine(notPayedRequest);
	// 				
	// 				var markupWithPayUrl = new InlineKeyboardMarkup(new[]
	// 				{
	// 					new[]
	// 					{
	// 						InlineKeyboardButton.WithUrl("Перейти для оплаты", payUrl),
	// 						InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu), 
	// 					}
	// 				});
	// 				await _telegramBotClient.EditMessageTextAsync(update.CallbackQuery.From.Id,
	// 					update.CallbackQuery.Message.MessageId, stringBuilder.ToString(), replyMarkup: markupWithPayUrl);
	// 				return;
	// 			}
	//
	// 			var existNotPayedRequest = "Запрос на оплату еще формируется, пожалуйста подождите";
	// 			stringBuilder.AppendLine(existNotPayedRequest);
	// 			await ProcessBackToMainMenu(update, user, stringBuilder.ToString());
	//
	// 			return;
	// 		}
	// 	}
	//
	// 	var infoMessage = "Для пополнения количества использований необходимо их оплатить." +
	// 	                  Environment.NewLine +
	// 	                  "Для оплаты нажните кнопку. После будет отправлена ссылка на оплату 10 использований." +
	// 	                  Environment.NewLine +
	// 	                  "После оплаты в течени некоторого времени количество использований будет пополнено";
	// 	stringBuilder.AppendLine(infoMessage);
	// 	var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
	// 	{
	// 		new[]
	// 		{
	// 			InlineKeyboardButton.WithCallbackData("Оплатить", CallbackDataConstants.GeneratePaymentRequest),
	// 			InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu),
	// 		}
	// 	});
	// 	await _telegramBotClient.EditMessageTextAsync(update.CallbackQuery.From.Id,
	// 		update.CallbackQuery.Message.MessageId, stringBuilder.ToString(),
	// 		replyMarkup: inlineKeyboardMarkup);
	// }
}