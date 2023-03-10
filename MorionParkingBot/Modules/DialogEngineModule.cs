using Autofac;
using MorionParkingBot.DialogEngine;

namespace MorionParkingBot.Modules;

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