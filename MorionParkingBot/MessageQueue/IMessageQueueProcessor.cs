namespace MorionParkingBot.MessageQueue;

public interface IMessageQueueProcessor
{
    void StartProcess();
    void StopProcess();
}