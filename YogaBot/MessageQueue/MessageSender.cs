using YogaBot.Frames;
using Telegram.Bot;

namespace YogaBot.MessageQueue;

public class MessageSender : IMessageSender
{
    private readonly TelegramBotClient telegramBotClient;
    private readonly IOutputMessageQueue outputMessageQueue;
    private CancellationTokenSource cancellationTokenSource;

    public MessageSender(TelegramBotClient telegramBotClient, IOutputMessageQueue outputMessageQueue)
    {
        this.telegramBotClient = telegramBotClient;
        this.outputMessageQueue = outputMessageQueue;
    }

    public void StartSending()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        cancellationTokenSource = new CancellationTokenSource();
        SendingTask(cancellationTokenSource.Token);
    }

    public void StopSending()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource = null;
    }

    private async void SendingTask(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = outputMessageQueue.GetMessage();
            if (message == null)
            {
                await Task.Delay(15, cancellationToken);
                continue;
            }

            if (message.MessageType == MessageType.Send)
            {
                if (message.Ikm != null)
                    await telegramBotClient.SendTextMessageAsync(message.ChatId, message.MessageText,
                        replyMarkup: message.Ikm, cancellationToken: cancellationToken);
                else
                    await telegramBotClient.SendTextMessageAsync(message.ChatId, message.MessageText,
                        cancellationToken: cancellationToken);
            }
            else
            {
                if (message.Ikm != null)
                    await telegramBotClient.EditMessageTextAsync(message.ChatId,
                        message.MessageId, message.MessageText, replyMarkup: message.Ikm,
                        cancellationToken: cancellationToken);
                else
                {
                    await telegramBotClient.EditMessageTextAsync(message.ChatId,
                        message.MessageId, message.MessageText, cancellationToken: cancellationToken);
                }
            }
        }
    }
}