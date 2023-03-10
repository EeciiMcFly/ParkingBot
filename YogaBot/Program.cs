using Autofac;
using Autofac.Extensions.DependencyInjection;
using YogaBot.Modules;

var builder = Host.CreateDefaultBuilder(args);

builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
	containerBuilder.RegisterModule<BotModule>();
	containerBuilder.RegisterModule<DialogEngineModule>();
	containerBuilder.RegisterModule<DialogsModule>();
	containerBuilder.RegisterModule<MessageProcessorsModules>();
	containerBuilder.RegisterModule<QueueModule>();
	containerBuilder.RegisterModule<StorageModule>();
}));

IHost host = builder
	.Build();

await host.RunAsync();