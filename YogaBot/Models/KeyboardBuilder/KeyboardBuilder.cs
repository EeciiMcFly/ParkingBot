using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Models.KeyboardBuilder;

public interface IKeyboardBuilder
{
    Task<List<InlineKeyboardButton[]>> BuildForArrangementFrameAsync(BotContext context, long arrangementGuid);
}

public class KeyboardBuilder : IKeyboardBuilder
{
    private readonly IUsersRepository usersRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;

    public KeyboardBuilder(IUsersRepository usersRepository, IUserArrangementRelationsRepository userArrangementRelationsRepository)
    {
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
    }

    public async Task<List<InlineKeyboardButton[]>> BuildForArrangementFrameAsync(BotContext context, long arrangementGuid)
    {
        var user = await usersRepository.GetUserAsync(context.TelegramUserId);
        var relationsForUser = await userArrangementRelationsRepository.GetRelationsForUserAsync(user.UserId);
        var role = relationsForUser.FirstOrDefault(e => e.ArrangementId == arrangementGuid)?.Role;

        var ikm = new List<InlineKeyboardButton[]>();
        ikm.Add(new[]
        {
            InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия",
                CallbackDataConstants.GetEvents + '/' + arrangementGuid)
        });
        if (role == Role.Admin || role == Role.Trainer)
        {
            ikm.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData("Запланировать занятие",
                    CallbackDataConstants.CreateEvent + '/' + arrangementGuid)
            });
            ikm.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData("Удалить занятие",
                    CallbackDataConstants.DeleteEvent + '/' + arrangementGuid)
            });
            ikm.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData("Сколько должны участники?",
                    CallbackDataConstants.CalculatePriceForAll + '/' + arrangementGuid)
            });
        }

        ikm.Add(new[]
        {
            InlineKeyboardButton.WithCallbackData("Сколько я должен?",
                CallbackDataConstants.CalculatePriceForRequester + '/' + arrangementGuid)
        });
        ikm.Add(new[] { InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities) });
        return ikm;
    }
}