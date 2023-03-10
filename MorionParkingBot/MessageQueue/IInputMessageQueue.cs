﻿namespace MorionParkingBot.MessageQueue;

public interface IInputMessageQueue
{
    public void AddMessage(BotContext messageContext);
    public BotContext GetMessage();
}