using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Metrics;

namespace MorionParkingBot.Modules;

public class UsingModules : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");

				var optionsBuilder = new DbContextOptionsBuilder<UsingDbContext>();
				optionsBuilder.UseNpgsql(connectionString);

				var dbContext = new UsingDbContext(optionsBuilder.Options);
				return new UsingRepository(dbContext);
			})
			.As<IUsingRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UsingService>()
			.As<IUsingService>()
			.InstancePerLifetimeScope();
	}
}