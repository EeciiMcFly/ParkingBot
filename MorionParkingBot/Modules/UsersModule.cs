using Autofac;
using Microsoft.EntityFrameworkCore;
using MorionParkingBot.Frames;
using MorionParkingBot.MessagesProcessors;
using MorionParkingBot.PromoCodes;
using MorionParkingBot.Users;
using Telegram.Bot;

namespace MorionParkingBot.Modules;

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
				var telegramBotClient = new TelegramBotClient("5702939362:AAE3HKbDkgrMKhRUzpVYEjToFUgt8WQi7s8");

				return telegramBotClient;
			})
			.As<TelegramBotClient>()
			.SingleInstance();

		builder.RegisterType<UsersService>()
			.As<IUsersService>()
			.InstancePerLifetimeScope();
	}
}