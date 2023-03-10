using Autofac;
using Telegram.Bot;

namespace YogaBot.Modules;

public class BotModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Worker>()
            .As<IHostedService>()
            .SingleInstance();

        builder.Register(c =>
            {
                var telegramBotClient = new TelegramBotClient("6159101214:AAFbD8_eGfgnuOiFLouSPpwDlZoOEwCrRUs");

                return telegramBotClient;
            })
            .As<TelegramBotClient>()
            .SingleInstance();
    }
}