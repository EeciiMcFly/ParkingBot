using System.Collections.Concurrent;
using MorionParkingBot.Frames;

namespace MorionParkingBot.MessageQueue;

public class OutputMessageQueue : IOutputMessageQueue
{
    private readonly ConcurrentQueue<FrameState> queue = new();

    public void AddMessage(FrameState frameState)
    {
        queue.Enqueue(frameState);
    }

    public FrameState GetMessage()
    {
        queue.TryDequeue(out var message);
        return message;
    }
}