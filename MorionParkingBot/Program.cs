using Autofac;
using Autofac.Extensions.DependencyInjection;
using MorionParkingBot.Modules;

var builder = Host.CreateDefaultBuilder(args);

builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
	containerBuilder.RegisterModule<UsersModule>();
	containerBuilder.RegisterModule<PromoCodesModule>();
	containerBuilder.RegisterModule<FramesModules>();
	containerBuilder.RegisterModule<MessageProcessorsModules>();
	containerBuilder.RegisterModule<ParkingsModule>();
	containerBuilder.RegisterModule<LoggingModule>();
	containerBuilder.RegisterModule<UsingModules>();
}));

IHost host = builder
	.Build();

await host.RunAsync();