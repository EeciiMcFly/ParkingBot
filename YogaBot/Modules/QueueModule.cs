using Autofac;
using Telegram.Bot;
using YogaBot.MessageQueue;

namespace YogaBot.Modules;

public class QueueModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
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