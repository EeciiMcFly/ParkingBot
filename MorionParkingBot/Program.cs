using Autofac;
using Autofac.Extensions.DependencyInjection;
using MorionParkingBot.Modules;

var builder = Host.CreateDefaultBuilder(args);

builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
	containerBuilder.RegisterModule<BotModule>();
	containerBuilder.RegisterModule<DialogEngineModule>();
	containerBuilder.RegisterModule<DialogsModule>();
	containerBuilder.RegisterModule<MessageProcessorsModules>();
	containerBuilder.RegisterModule<QueueModule>();
}));

IHost host = builder
	.Build();

await host.RunAsync();