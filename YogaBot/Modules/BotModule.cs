using Autofac;
using Telegram.Bot;
using YogaBot.Models.ChatMember;

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
                var configuration = c.Resolve<IConfiguration>();
                ChatMemberProcessor.CurrentBotId = Convert.ToInt64(configuration.GetConnectionString("BotId"));
                var telegramApiKey = configuration.GetConnectionString("TelegramApiKey");
                var telegramBotClient = new TelegramBotClient(telegramApiKey);

                return telegramBotClient;
            })
            .As<TelegramBotClient>()
            .SingleInstance();
    }
}