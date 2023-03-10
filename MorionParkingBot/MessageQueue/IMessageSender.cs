namespace MorionParkingBot.MessageQueue;

public interface IMessageSender
{
    void StartSending();
    void StopSending();
}