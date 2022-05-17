using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Parkings;
using MorionParkingBot.Parkings.ToServerServices;

namespace MorionParkingBot.Modules;

public class ParkingsModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");

				var optionsBuilder = new DbContextOptionsBuilder<ParkingDbContext>();
				optionsBuilder.UseNpgsql(connectionString);
			
				var dbContext = new ParkingDbContext(optionsBuilder.Options);
				return new ParkingsRepository(dbContext);
			})
			.As<IParkingsRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ParkingsService>()
			.As<IParkingsService>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ServerInfoProvider>()
			.As<ServerInfoProvider>()
			.SingleInstance();
	}
}