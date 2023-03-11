using Autofac;
using YogaBot.Dialogs;
using YogaBot.Dialogs.ArrangementProcessings;
using YogaBot.Dialogs.EventProcessings;
using YogaBot.Dialogs.EventProcessings.Calculations;

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

        builder.RegisterType<DeleteEventsDialog>()
            .As<DeleteEventsDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();

        builder.RegisterType<GetEventsDialog>()
            .As<GetEventsDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();

        builder.RegisterType<CalculateForRequesterEventDialog>()
            .As<CalculateForRequesterEventDialog>()
            .As<IDialog<BotContext>>()
            .SingleInstance();

        builder.RegisterType<CalculateForAllDialog>()
            .As<CalculateForAllDialog>()
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
        
        builder.RegisterType<NewDeleteEvent>()
            .As<NewDeleteEvent>()
            .As<IDialog<BotContext>>()
            .SingleInstance();
    }
}