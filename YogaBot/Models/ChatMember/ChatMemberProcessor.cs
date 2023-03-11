using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;
using User = YogaBot.Storage.Users.User;

namespace YogaBot.Models.ChatMember;

public interface IChatMemberProcessor
{
    void ProcessChatAsync(Update update);
    void ProcessChatMember(Update update);
}

public class ChatMemberProcessor : IChatMemberProcessor
{
    public static long CurrentBotId { get; set; }
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUsersRepository usersRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;
    private readonly IEventsRepository eventsRepository;
    private readonly IPresenceRepository presenceRepository;

    public ChatMemberProcessor(IArrangementRepository arrangementRepository,
        IUsersRepository usersRepository,
        IUserArrangementRelationsRepository userArrangementRelationsRepository,
        IEventsRepository eventsRepository,
        IPresenceRepository presenceRepository)
    {
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
        this.eventsRepository = eventsRepository;
        this.presenceRepository = presenceRepository;
    }

    public async void ProcessChatAsync(Update update)
    {
        var updateMyChatMember = update.MyChatMember;
        var newChatMember = updateMyChatMember.NewChatMember;
        if (newChatMember.User.Id != CurrentBotId)
            return;

        switch (newChatMember.Status)
        {
            case ChatMemberStatus.Creator:
                break;
            case ChatMemberStatus.Administrator:
                var newArrangement = new Arrangement
                {
                    Name = updateMyChatMember.Chat.Title,
                    ChatId = updateMyChatMember.Chat.Id
                };

                var createdArrangement = await arrangementRepository.AddArrangementAsync(newArrangement);
                var userAdmin = await usersRepository.GetUserAsync(updateMyChatMember.From.Id);
                if (userAdmin == null)
                {
                    var newUser = new User
                    {
                        TelegramUserId = updateMyChatMember.From.Id,
                    };
                    userAdmin = await usersRepository.AddUserAsync(newUser);
                }
                var newUserArrangementRelation = new UserArrangementRelation
                {
                    ArrangementId = createdArrangement.ArrangementId,
                    UserId = userAdmin.UserId,
                    Role = Role.Admin
                };
                await userArrangementRelationsRepository.AddUserEventRelationAsync(newUserArrangementRelation);

                return;
            case ChatMemberStatus.Member:
                break;
            case ChatMemberStatus.Kicked:
            case ChatMemberStatus.Left:
                var arrangement = await arrangementRepository.GetArrangementByChatIdAsync(updateMyChatMember.Chat.Id);
                if (arrangement != null)
                {
                    await arrangementRepository.DeleteArrangementAsync(arrangement);
                    await userArrangementRelationsRepository.DeleteRelationByArrangementId(arrangement.ArrangementId);
                    var eventsForArrangement =
                        await eventsRepository.GetEventsForArrangementAsync(arrangement.ArrangementId);
                    foreach (var @event in eventsForArrangement)
                    {
                        await presenceRepository.DeletePresenceForEventAsync(@event.EventId);
                        await eventsRepository.DeleteEventAsync(@event);
                    }
                }

                return;
            case ChatMemberStatus.Restricted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async void ProcessChatMember(Update update)
    {
        var message = update.Message;
        switch (message.Type)
        {
            case MessageType.ChatMemberLeft:
            {
                var leftChatMember = message.LeftChatMember;
                var user = await usersRepository.GetUserAsync(leftChatMember.Id);
                var arrangement = await arrangementRepository.GetArrangementByChatIdAsync(message.Chat.Id);
                if (arrangement != null && user != null)
                {
                    await userArrangementRelationsRepository.DeleteRelationByArrangementIdAndUserId(
                        arrangement.ArrangementId, user.UserId);
                    var eventsForArrangement =
                        await eventsRepository.GetEventsForArrangementAsync(arrangement.ArrangementId);
                    foreach (var @event in eventsForArrangement)
                    {
                        await presenceRepository.DeletePresenceForEventAndUserAsync(@event.EventId, user.UserId);
                    }
                }

                break;
            }
            case MessageType.ChatMembersAdded:
            {
                var newChatMembers = message.NewChatMembers;
                foreach (var newChatMember in newChatMembers)
                {
                    var user = await usersRepository.GetUserAsync(newChatMember.Id);
                    if (user == null)
                    {
                        var newUser = new User
                        {
                            TelegramUserId = newChatMember.Id,
                        };
                        user = await usersRepository.AddUserAsync(newUser);
                    }

                    var arrangement = await arrangementRepository.GetArrangementByChatIdAsync(message.Chat.Id);
                    if (arrangement == null || user == null)
                        continue;

                    var newUserArrangementRelation = new UserArrangementRelation
                    {
                        ArrangementId = arrangement.ArrangementId,
                        UserId = user.UserId,
                        Role = Role.Trained
                    };
                    await userArrangementRelationsRepository.AddUserEventRelationAsync(newUserArrangementRelation);
                }

                return;
            }
        }
    }
}