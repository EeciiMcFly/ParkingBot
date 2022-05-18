using Autofac;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MorionParkingBot.Modules;

public class LoggingModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.Register(c =>
			{
				var logger = new LoggerConfiguration()
					.MinimumLevel.Debug()
					.WriteTo.File($"logs/{DateTime.Now.ToString("yy-MM-dd")}.log")
					.CreateLogger();

				return logger;
			})
			.As<ILogger>()
			.SingleInstance();
	}
}