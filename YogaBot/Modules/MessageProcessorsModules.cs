using Autofac;
using YogaBot.MessagesProcessors;

namespace YogaBot.Modules;

public class MessageProcessorsModules : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CallbackQueryProcessor>()
            .As<CallbackQueryProcessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<MessagesProcessor>()
            .As<MessagesProcessor>()
            .InstancePerLifetimeScope();
    }
}