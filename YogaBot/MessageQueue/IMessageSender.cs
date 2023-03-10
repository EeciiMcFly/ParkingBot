namespace YogaBot.MessageQueue;

public interface IMessageSender
{
    void StartSending();
    void StopSending();
}