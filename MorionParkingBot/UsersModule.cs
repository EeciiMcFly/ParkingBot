using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Database;
using MorionParkingBot.MessagesProcessors;
using MorionParkingBot.PromoCodes;
using MorionParkingBot.Users;
using Telegram.Bot;

namespace MorionParkingBot;

public class UsersModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<Worker>()
			.As<IHostedService>()
			.SingleInstance();

		builder.Register(c =>
			{
				var configuration = c.Resolve<IConfiguration>();
				var connectionString = configuration.GetConnectionString("DatabaseConnectionTemplateWithoutDbName");

				var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
				optionsBuilder.UseNpgsql(connectionString);
			
				var dbContext = new UserDbContext(optionsBuilder.Options);
				return new UsersRepository(dbContext);
			})
			.As<IUsersRepository>()
			.InstancePerLifetimeScope();
		
		builder.Register(c =>
			{
				var telegramBotClient = new TelegramBotClient("5257347533:AAH_EZtoPXtadAkchdMbCzP-h6t4SQhLjFc");
				//var telegramBotClient = new TelegramBotClient("5225586467:AAH4AKYDeJc0tIaBggMv_Arhz3uVx7giJK8");
				
				return telegramBotClient;
			})
			.As<TelegramBotClient>()
			.SingleInstance();

		builder.RegisterType<CallbackQueryProcessor>()
			.As<CallbackQueryProcessor>()
			.InstancePerLifetimeScope();

		builder.RegisterType<MessagesProcessor>()
			.As<MessagesProcessor>()
			.InstancePerLifetimeScope();

		builder.RegisterType<UsersService>()
			.As<IUsersService>()
			.InstancePerLifetimeScope();

		builder.RegisterType<ParkingRequestQueue>()
			.As<ParkingRequestQueue>()
			.SingleInstance();

		builder.RegisterType<FrameStateLogic>()
			.As<FrameStateLogic>()
			.SingleInstance();

		builder.RegisterType<FrameStateConstructor>()
			.As<FrameStateConstructor>()
			.SingleInstance();

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

		builder.RegisterType<PromoCodeService>()
			.As<IPromoCodeService>()
			.InstancePerLifetimeScope();
	}
}