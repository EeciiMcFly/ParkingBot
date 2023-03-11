using System.Text;
using Telegram.Bot;
using YogaBot.Frames;
using YogaBot.Models.Poll;
using YogaBot.Storage.Events;

namespace YogaBot.BackgroundWorkers;

public class PollCreateWorker : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly TelegramBotClient telegramBotClient;
    private CancellationTokenSource cancellationTokenSource = new();

    public PollCreateWorker(IServiceProvider serviceProvider, TelegramBotClient telegramBotClient)
    {
        this.serviceProvider = serviceProvider;
        this.telegramBotClient = telegramBotClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Polling();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource.Cancel();
    }

    private async Task Polling()
    {
        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            var date = new DateTime(utcNow.Year, utcNow.Month, utcNow.AddDays(1).Day, 11, 59, 0, DateTimeKind.Utc);
            using var scope = serviceProvider.CreateAsyncScope();
            var eventsRepository = scope.ServiceProvider.GetService(typeof(IEventsRepository)) as IEventsRepository;
            var events = await eventsRepository.GetEventsForDate(date);

            if (events.Any())
            {
                foreach (var @event in events)
                {
                    if (@event.PollSend)
                        continue;

                    if (@event.Date.Subtract(DateTime.UtcNow.AddDays(1)).Duration() < TimeSpan.FromMinutes(5))
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(@event.Name);
                        if (@event.Cost > 0)
                            stringBuilder.AppendLine($"Стоимость: {@event.Cost}");
                        stringBuilder.AppendLine("Идешь?");
                        var poll = new Poll
                        {
                            Answer = stringBuilder.ToString(),
                            Options = new[] { "Да", "Нет" },
                            AllowsMultipleAnswers = false,
                            IsAnonymous = false,
                            CloseDate = DateTime.UtcNow.AddHours(8)
                        };

                        var messageWithPoll = new FrameState
                        {
                            Poll = poll,
                            ChatId = @event.Arrangement.ChatId,
                            MessageType = MessageType.Poll
                        };

                        var pool = messageWithPoll.Poll;
                        var message = await telegramBotClient.SendPollAsync(messageWithPoll.ChatId, pool.Answer, pool.Options,
                            isAnonymous: pool.IsAnonymous, allowsMultipleAnswers: pool.AllowsMultipleAnswers);

                        @event.PollSend = true;
                        @event.PollMessageId = message.MessageId;
                        @event.PollId = message.Poll.Id;
                        await eventsRepository.UpdateAsync(@event);
                    }
                }
            }

            await Task.Delay(1000 * 60, cancellationTokenSource.Token);
        }
    }
}