using Telegram.Bot.Types;
using YogaBot.Storage.Events;
using YogaBot.Storage.Presences;
using YogaBot.Storage.Users;

namespace YogaBot.Models.Poll;

public interface IPollAnswerProcessor
{
    Task ProcessAnswer(Update update);
}

public class PollAnswerProcessor : IPollAnswerProcessor
{
    private readonly IEventsRepository eventsRepository;
    private readonly IPresenceRepository presenceRepository;
    private readonly IUsersRepository usersRepository;

    public PollAnswerProcessor(IEventsRepository eventsRepository, IPresenceRepository presenceRepository,
        IUsersRepository usersRepository)
    {
        this.eventsRepository = eventsRepository;
        this.presenceRepository = presenceRepository;
        this.usersRepository = usersRepository;
    }

    public async Task ProcessAnswer(Update update)
    {
        var answer = update.PollAnswer.OptionIds.FirstOrDefault(defaultValue: 1);
        var eventByPollId = await eventsRepository.GetEventByPollIdAsync(update.PollAnswer.PollId);
        var user = await usersRepository.GetUserAsync(update.PollAnswer.User.Id);
        if (eventByPollId != null && user != null)
        {
            if (answer == 0)
            {
                var presence = new Presence
                {
                    UserId = user.UserId,
                    EventId = eventByPollId.EventId
                };
                await presenceRepository.AddPresenceAsync(presence);
            }
            else
            {
                await presenceRepository.DeletePresenceForEventAndUserAsync(eventByPollId.EventId, user.UserId);
            }
        }
    }
}