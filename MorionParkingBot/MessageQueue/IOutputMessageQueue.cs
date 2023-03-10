using MorionParkingBot.Frames;

namespace MorionParkingBot.MessageQueue;

public interface IOutputMessageQueue
{
    public void AddMessage(FrameState frameState);
    public FrameState GetMessage();
}