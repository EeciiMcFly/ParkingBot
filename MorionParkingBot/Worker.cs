using MorionParkingBot.MessagesProcessors;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MorionParkingBot;

public class Worker : IHostedService
{
	private readonly TelegramBotClient _telegramBotClient;
	private readonly IServiceProvider _serviceProvider;

	public Worker(TelegramBotClient telegramBotClient, IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_telegramBotClient = telegramBotClient;
	}

	private async Task HandleMessage(ITelegramBotClient telegramBotClient, Update update, CancellationToken arg3)
	{
		try
		{
			if (update.Message != null)
			{
				using var scope = _serviceProvider.CreateAsyncScope();
				var messagesProcessor = scope.ServiceProvider.GetService(typeof(MessagesProcessor)) as MessagesProcessor;
				await messagesProcessor.ProcessMessage(update);
				return;
			}

			if (update.Type == UpdateType.CallbackQuery)
			{
				using var scope = _serviceProvider.CreateAsyncScope();
				var callbackQueryProcessor = scope.ServiceProvider.GetService(typeof(CallbackQueryProcessor)) as CallbackQueryProcessor;
				//await callbackQueryProcessor.ProcessCallbackQuery(update);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}

	private async Task ErrorHandler(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken arg3)
	{
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		var defaultUpdateHandler = new DefaultUpdateHandler(HandleMessage, ErrorHandler);
		await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
		_telegramBotClient.StartReceiving(defaultUpdateHandler, cancellationToken: cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		return;
	}
}