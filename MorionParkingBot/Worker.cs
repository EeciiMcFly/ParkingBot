using MorionParkingBot.MessageQueue;
using MorionParkingBot.MessagesProcessors;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MorionParkingBot;

public class Worker : IHostedService
{
	private readonly TelegramBotClient telegramBotClient;
	private readonly IServiceProvider serviceProvider;
	private readonly IInputMessageQueue inputMessageQueue;
	private readonly IMessageSender messageSender;
	private readonly IMessageQueueProcessor messagesProcessor;

	public Worker(TelegramBotClient telegramBotClient, IServiceProvider serviceProvider,
		IInputMessageQueue inputMessageQueue, IMessageSender messageSender,
		IMessageQueueProcessor messagesProcessor)
	{
		this.serviceProvider = serviceProvider;
		this.telegramBotClient = telegramBotClient;
		this.inputMessageQueue = inputMessageQueue;
		this.messageSender = messageSender;
		this.messagesProcessor = messagesProcessor;
	}

	private async Task HandleMessage(ITelegramBotClient telegramBotClient, Update update, CancellationToken arg3)
	{
		try
		{
			if (update.Message != null)
			{
				var messageContext = new BotContext
				{
					ChatId = update.Message.Chat.Id,
					TelegramUserId = update.Message.From.Id,
					MessageText = update.Message.Text
				};

				inputMessageQueue.AddMessage(messageContext);

				return;
			}

			if (update.Type == UpdateType.CallbackQuery)
			{
				var messageContext = new BotContext
				{
					ChatId = update.CallbackQuery.From.Id,
					TelegramUserId = update.CallbackQuery.From.Id,
					MessageId = update.CallbackQuery.Message.MessageId,
					CallbackData = update.CallbackQuery.Data
				};
				
				inputMessageQueue.AddMessage(messageContext);
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
		await telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
		telegramBotClient.StartReceiving(defaultUpdateHandler, cancellationToken: cancellationToken);
		messageSender.StartSending();
		messagesProcessor.StartProcess();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		return;
	}
}