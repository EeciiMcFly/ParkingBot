using MorionParkingBot.MessagesProcessors;

namespace MorionParkingBot.MessageQueue;

public class MessageQueueProcessor : IMessageQueueProcessor
{
    private readonly IInputMessageQueue inputMessageQueue;
    private readonly IServiceProvider serviceProvider;

    private CancellationTokenSource cancellationTokenSource;

    public MessageQueueProcessor(IInputMessageQueue inputMessageQueue, IServiceProvider serviceProvider)
    {
        this.inputMessageQueue = inputMessageQueue;
        this.serviceProvider = serviceProvider;
    }
    
    public void StartProcess()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        cancellationTokenSource = new CancellationTokenSource();
        ProcessTask(cancellationTokenSource.Token);
    }
    
    public void StopProcess()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource = null;
    }

    private async void ProcessTask(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = inputMessageQueue.GetMessage();
            if (message == null)
            {
                await Task.Delay(15, cancellationToken);
                continue;
            }

            using var scope = serviceProvider.CreateAsyncScope();
            
            if (message.CallbackData != null)
            {
                var callbackQueryProcessor = scope.ServiceProvider.GetService(typeof(CallbackQueryProcessor)) as CallbackQueryProcessor;
                await callbackQueryProcessor.ProcessCallbackQuery(message);
            }
            else
            {
                var messagesProcessor = scope.ServiceProvider.GetService(typeof(MessagesProcessor)) as MessagesProcessor; 
                await messagesProcessor.ProcessMessage(message);
            }
        }
    }
}