using Autofac;
using YogaBot.Models.ChatMember;

namespace YogaBot.Modules;

public class ModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ChatMemberProcessor>()
            .As<IChatMemberProcessor>()
            .SingleInstance();
    }
}