using Autofac;
using Autofac.Extensions.DependencyInjection;
using MorionParkingBot;

var builder = Host.CreateDefaultBuilder(args);

builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
	containerBuilder.RegisterModule<UsersModule>();
}));

IHost host = builder
	.Build();

await host.RunAsync();