using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.ParkingSchedule;

namespace MorionParkingBot.Modules;

public class ParkingScheduleModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");

				var optionsBuilder = new DbContextOptionsBuilder<ParkingScheduleDbContext>();
				optionsBuilder.UseNpgsql(connectionString);

				var dbContext = new ParkingScheduleDbContext(optionsBuilder.Options);
				return new ParkingScheduleRepository(dbContext);
			})
			.As<IParkingScheduleRepository>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ParkingScheduleService>()
			.As<ParkingScheduleService>()
			.InstancePerLifetimeScope();
	}
}