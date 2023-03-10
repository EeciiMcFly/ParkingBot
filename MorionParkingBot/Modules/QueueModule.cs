using Autofac;
using MorionParkingBot.MessageQueue;
using MorionParkingBot.MessagesProcessors;
using Telegram.Bot;

namespace MorionParkingBot.Modules;

public class QueueModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c =>
            {
                var telegramBotClient = new TelegramBotClient("");

                return telegramBotClient;
            })
            .As<TelegramBotClient>()
            .SingleInstance();
        
        builder.RegisterType<InputMessageQueue>()
            .As<IInputMessageQueue>()
            .SingleInstance();

        builder.RegisterType<OutputMessageQueue>()
            .As<IOutputMessageQueue>()
            .SingleInstance();

        builder.RegisterType<MessageSender>()
            .As<IMessageSender>()
            .SingleInstance();

        builder.RegisterType<MessageQueueProcessor>()
            .As<IMessageQueueProcessor>()
            .SingleInstance();
    }
}