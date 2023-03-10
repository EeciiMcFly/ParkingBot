using Autofac;
using Telegram.Bot;

namespace MorionParkingBot.Modules;

public class BotModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Worker>()
            .As<IHostedService>()
            .SingleInstance();

        builder.Register(c =>
            {
                var telegramBotClient = new TelegramBotClient("5702939362:AAE3HKbDkgrMKhRUzpVYEjToFUgt8WQi7s8");

                return telegramBotClient;
            })
            .As<TelegramBotClient>()
            .SingleInstance();
    }
}