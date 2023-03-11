﻿using Autofac;
using YogaBot.Models.ChatMember;
using YogaBot.Models.Poll;

namespace YogaBot.Modules;

public class ModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ChatMemberProcessor>()
            .As<IChatMemberProcessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<PollAnswerProcessor>()
            .As<IPollAnswerProcessor>()
            .InstancePerLifetimeScope();
    }
}