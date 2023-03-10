namespace YogaBot.MessageQueue;

public interface IMessageQueueProcessor
{
    void StartProcess();
    void StopProcess();
}