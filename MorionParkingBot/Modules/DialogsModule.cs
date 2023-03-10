using Autofac;
using MorionParkingBot.Dialogs;

namespace MorionParkingBot.Modules;

public class DialogsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DefaultDialog>()
            .As<DefaultDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
        
        builder.RegisterType<CreateEventDialog>()
            .As<CreateEventDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
        
        builder.RegisterType<ActivityDialog>()
            .As<ActivityDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
    }
}