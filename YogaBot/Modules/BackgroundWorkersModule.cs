using Autofac;
using YogaBot.BackgroundWorkers;

namespace YogaBot.Modules;

public class BackgroundWorkersModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PollCreateWorker>()
            .As<IHostedService>()
            .SingleInstance();
    }
}