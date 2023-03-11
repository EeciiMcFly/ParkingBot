using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Events
{
    public interface IEventsRepository
    {
        Task<EventData?> GetEventAsync(Guid eventId);

        Task<IEnumerable<EventData?>> GetEventsForDateAsync(DateTime userId);
        
        Task<IEnumerable<EventData?>> GetEventsForArrangementAsync(Guid arrangementId);

        Task AddEventAsync(EventData eventData);

        Task DeleteEventAsync(Guid eventId);
    }
    
    public class EventsRepository : IEventsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public EventsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<EventData?> GetEventAsync(Guid eventId)
        {
            var eventData = await _userDbContext.Events.FirstOrDefaultAsync(data => data.Id == eventId);

            return eventData;
        }

        public async Task<IEnumerable<EventData?>> GetEventsForDateAsync(DateTime date)
        {
            var events = await _userDbContext.Events.Where(data => data.Date == date).ToListAsync();

            return events;
        }
        
        public async Task<IEnumerable<EventData?>> GetEventsForArrangementAsync(Guid arrangementId)
        {
            var events = await _userDbContext.Events.Where(data => data.ArrangementId == arrangementId).ToListAsync();

            return events;
        }
        
        public async Task DeleteEventAsync(Guid eventId)
        {
            var deletedEvent = await _userDbContext.Events.FirstOrDefaultAsync(data => data.Id == eventId);
            var eventData = _userDbContext.Events.Remove(deletedEvent);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task AddEventAsync(EventData eventData)
        {
            await _userDbContext.Events.AddAsync(eventData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}