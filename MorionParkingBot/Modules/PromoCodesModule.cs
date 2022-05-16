using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.PromoCodes;

namespace MorionParkingBot.Modules;

public class PromoCodesModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<PromoCodeService>()
			.As<IPromoCodeService>()
			.InstancePerLifetimeScope();
		
		builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");

				var optionsBuilder = new DbContextOptionsBuilder<PromoCodeDbContext>();
				optionsBuilder.UseNpgsql(connectionString);
			
				var dbContext = new PromoCodeDbContext(optionsBuilder.Options);
				return new PromoCodeRepository(dbContext);
			})
			.As<IPromoCodeRepository>()
			.InstancePerLifetimeScope();
	}
}