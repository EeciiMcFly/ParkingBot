using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Presences
{
    public interface IPresenceRepository
    {
        Task<IEnumerable<Presence?>> GetPresencesForUserAsync(long userId);

        Task<IEnumerable<Presence?>> GetPresencesForEventAsync(long eventId);

        Task AddPresenceAsync(Presence presence);
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
            var relationsData = await _userDbContext.Presences.Where(data => data.EventId == eventId).ToListAsync();

            return relationsData;
        }

        public async Task AddPresenceAsync(Presence presence)
        {
            await _userDbContext.Presences.AddAsync(presence);

            await _userDbContext.SaveChangesAsync();
        }
    }
}