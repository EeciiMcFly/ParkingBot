using Autofac;
using Autofac.Extensions.DependencyInjection;
using YogaBot.Modules;

var builder = Host.CreateDefaultBuilder(args);

builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
	containerBuilder.RegisterModule<MessageProcessorsModules>();
	containerBuilder.RegisterModule<BotModule>();
	containerBuilder.RegisterModule<QueueModule>();
}));

IHost host = builder
	.Build();

await host.RunAsync();