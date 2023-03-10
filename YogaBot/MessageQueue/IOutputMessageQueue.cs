using YogaBot.Frames;

namespace YogaBot.MessageQueue;

public interface IOutputMessageQueue
{
    public void AddMessage(FrameState frameState);
    public FrameState GetMessage();
}