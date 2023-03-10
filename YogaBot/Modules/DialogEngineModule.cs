using Autofac;
using YogaBot.DialogEngine;

namespace YogaBot.Modules;

public class DialogEngineModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DialogSelector>()
            .As<IDialogSelector>()
            .SingleInstance();
        
        builder.RegisterType<DialogStateStorage>()
            .As<IDialogStateStorage>()
            .As<IDialogStateSetter>()
            .SingleInstance();
    }
}