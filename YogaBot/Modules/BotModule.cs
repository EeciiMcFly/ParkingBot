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
                var telegramBotClient = new TelegramBotClient("5702939362:AAFL8LNR_S4HK_YaLTcMgtvK4yg-IBG3_0U");

                return telegramBotClient;
            })
            .As<TelegramBotClient>()
            .SingleInstance();
    }
}