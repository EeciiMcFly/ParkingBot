using System.Collections.Concurrent;

namespace YogaBot.MessageQueue;

public class InputMessageQueue : IInputMessageQueue
{
    private readonly ConcurrentQueue<BotContext> queue = new();

    public void AddMessage(BotContext messageContext)
    {
        queue.Enqueue(messageContext);
    }

    public BotContext GetMessage()
    {
        queue.TryDequeue(out var message);
        return message;
    }
}