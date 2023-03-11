using Autofac;
using YogaBot.Dialogs;
using YogaBot.Dialogs.ArrangementProcessings;
using YogaBot.Dialogs.EventProcessings;

namespace YogaBot.Modules;

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
        
        builder.RegisterType<DeleteEventDialog>()
            .As<DeleteEventDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
        
        builder.RegisterType<GetEventsDialog>()
            .As<GetEventsDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();

        builder.RegisterType<ArrangementDialog>()
            .As<ArrangementDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();

        builder.RegisterType<GetArrangementsDialog>()
            .As<GetArrangementsDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
    }
}