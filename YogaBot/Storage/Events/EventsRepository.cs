using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Events
{
    public interface IEventsRepository
    {
        Task<EventData?> GetDataAsync(long eventId);

        Task AddEventAsync(EventData eventData);
    }
    
    public class EventsRepository : IEventsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public EventsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<EventData?> GetDataAsync(long eventId)
        {
            var eventData = await _userDbContext.Events.FirstOrDefaultAsync(data => data.Id == eventId);

            return eventData;
        }

        public async Task AddEventAsync(EventData eventData)
        {
            await _userDbContext.Events.AddAsync(eventData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}