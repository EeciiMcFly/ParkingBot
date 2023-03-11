using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.Poll;

namespace YogaBot.BackgroundWorkers;

public class PollCreateWorker : IHostedService
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private CancellationTokenSource cancellationTokenSource = new ();

    public PollCreateWorker(IOutputMessageQueue outputMessageQueue)
    {
        this.outputMessageQueue = outputMessageQueue;
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
            //проверяем что надо отправить голосование
            if (false)
            {
                var poll = new Poll
                {
                    Answer = "Текст вопроса",
                    Options = new[] { "Ответ 1", "Ответ 2" },
                    AllowsMultipleAnswers = false,
                    IsAnonymous = false,
                    CloseDate = DateTime.UtcNow.AddHours(4)
                };

                var message = new FrameState
                {
                    Poll = poll,
                    ChatId = 12345 //id чата из базы,
                };
                
                outputMessageQueue.AddMessage(message);
            }

            await Task.Delay(1000 * 60, cancellationTokenSource.Token);
        }
    }
}