using Telegram.Bot;
using Telegram.Bot.Types;

namespace YogaBot.Models.InfoProvider;

public interface IInfoProvider
{
    Task<Telegram.Bot.Types.ChatMember> GetChatMember(long chatId, long userId);
}

public class InfoProvider : IInfoProvider
{
    private readonly TelegramBotClient telegramBotClient;

    public InfoProvider(TelegramBotClient telegramBotClient)
    {
        this.telegramBotClient = telegramBotClient;
    }

    public async Task<Telegram.Bot.Types.ChatMember> GetChatMember(long chatId, long userId)
    {
        var chatIdValue = new ChatId(chatId);
        var chatMember = await telegramBotClient.GetChatMemberAsync(chatIdValue, userId);

        return chatMember;
    }
}