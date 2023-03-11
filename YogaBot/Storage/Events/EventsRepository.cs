using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Events
{
    public interface IEventsRepository
    {
        Task<Event?> GetEventAsync(long eventId);

        Task<IEnumerable<Event?>> GetEventsForDateAsync(DateTime userId);
        
        Task<IEnumerable<Event?>> GetEventsForArrangementAsync(long arrangementId);

        Task AddEventAsync(Event @event);

        Task DeleteEventAsync(long eventId);
    }
    
    public class EventsRepository : IEventsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public EventsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Event?> GetEventAsync(long eventId)
        {
            var eventData = await _userDbContext.Events.FirstOrDefaultAsync(data => data.EventId == eventId);

            return eventData;
        }

        public async Task<IEnumerable<Event?>> GetEventsForDateAsync(DateTime date)
        {
            var events = await _userDbContext.Events.Where(data => data.Date == date).ToListAsync();

            return events;
        }
        
        public async Task<IEnumerable<Event?>> GetEventsForArrangementAsync(long arrangementId)
        {
            var events = await _userDbContext.Events.Where(data => data.ArrangementId == arrangementId).ToListAsync();

            return events;
        }
        
        public async Task DeleteEventAsync(long eventId)
        {
            var deletedEvent = await _userDbContext.Events.FirstOrDefaultAsync(data => data.EventId == eventId);
            var eventData = _userDbContext.Events.Remove(deletedEvent);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task AddEventAsync(Event @event)
        {
            await _userDbContext.Events.AddAsync(@event);

            await _userDbContext.SaveChangesAsync();
        }
    }
}