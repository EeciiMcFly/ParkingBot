using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Presences
{
    public interface IPresenceRepository
    {
        Task<IEnumerable<Presence?>> GetPresencesForUserAsync(long userId);

        Task<IEnumerable<Presence?>> GetPresencesForEventAsync(long eventId);

        Task AddPresenceAsync(Presence presence);

        Task DeletePresenceForEventAsync(long eventId);
        Task DeletePresenceForEventAndUserAsync(long eventId, long userId);
    }

    public class PresenceRepository : IPresenceRepository
    {
        private readonly StorageDbContext _userDbContext;

        public PresenceRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<Presence?>> GetPresencesForUserAsync(long userId)
        {
            var relationsData = await _userDbContext.Presences.Where(data => data.UserId == userId).ToListAsync();

            return relationsData;
        }
        
        public async Task<IEnumerable<Presence?>> GetPresencesForEventAsync(long eventId)
        {
            var relationsData = await _userDbContext.Presences.Include(e => e.User).Include(e => e.Event).Where(data => data.EventId == eventId).ToListAsync();

            return relationsData;
        }

        public async Task AddPresenceAsync(Presence presence)
        {
            await _userDbContext.Presences.AddAsync(presence);

            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeletePresenceForEventAsync(long eventId)
        {
            var presencesToDelete = await _userDbContext.Presences.Where(data => data.EventId == eventId).ToListAsync();
            _userDbContext.RemoveRange(presencesToDelete);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeletePresenceForEventAndUserAsync(long eventId, long userId)
        {
            var presencesToDelete = await _userDbContext.Presences.Where(data => data.EventId == eventId && data.UserId == userId).ToListAsync();
            _userDbContext.RemoveRange(presencesToDelete);
            await _userDbContext.SaveChangesAsync();
        }
    }
}