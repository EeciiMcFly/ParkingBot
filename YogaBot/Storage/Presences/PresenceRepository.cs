using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Presences
{
    public interface IPresenceRepository
    {
        Task<IEnumerable<PresenceData?>> GetPresencesForUserAsync(Guid userId);

        Task<IEnumerable<PresenceData?>> GetPresencesForEventAsync(Guid eventId);

        Task AddPresenceAsync(PresenceData presenceData);
    }

    public class PresenceRepository : IPresenceRepository
    {
        private readonly StorageDbContext _userDbContext;

        public PresenceRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<PresenceData?>> GetPresencesForUserAsync(Guid userId)
        {
            var relationsData = await _userDbContext.Presences.Where(data => data.UserId == userId).ToListAsync();

            return relationsData;
        }
        
        public async Task<IEnumerable<PresenceData?>> GetPresencesForEventAsync(Guid eventId)
        {
            var relationsData = await _userDbContext.Presences.Where(data => data.EventId == eventId).ToListAsync();

            return relationsData;
        }

        public async Task AddPresenceAsync(PresenceData presenceData)
        {
            await _userDbContext.Presences.AddAsync(presenceData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}