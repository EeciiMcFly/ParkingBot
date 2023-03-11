using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace YogaBot.Models.ChatMember;

public interface IChatMemberProcessor
{
    void ProcessChat(Update update);
    void ProcessChatMember(Update update);
}

public class ChatMemberProcessor : IChatMemberProcessor
{
    private readonly long currentBotId = 5702939362;

    public void ProcessChat(Update update)
    {
        if (update.MyChatMember.NewChatMember.User.Id == currentBotId)
        {
            if (update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Administrator)
            {
                // создать мероприятие
                // chatId = update.MyChatMember.Chat.ChatId
                // создать связь пользователя и события, где пользователь админ
                // получить всех членов группы???
            }

            if (update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Left)
            {
                //удалить мероприятие
                //удалить связи
                //удалить события
                //удалить посещения
            }
        }
    }

    public void ProcessChatMember(Update update)
    {
        var message = update.Message;
        if (message.Type == MessageType.ChatMemberLeft)
        {
            var leftChatMember = message.LeftChatMember;
            //удалить пользователя из мероприятия
            //удалить связь
            //удалить посещения
        }
        else if (message.Type == MessageType.ChatMembersAdded)
        {
            var newChatMembers = message.NewChatMembers;
            //добавить пользователя из мероприятия
            //создать связь
        }
    }
}