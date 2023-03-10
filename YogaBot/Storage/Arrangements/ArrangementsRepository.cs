using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Arrangements
{
    public interface IArrangementRepository
    {
        Task<IEnumerable<ArrangementData?>> GetArrangementForUserAsync(long userId);

        Task AddArrangementAsync(ArrangementData userData);
    }
    
    public class ArrangementRepository : IArrangementRepository
    {
        private readonly StorageDbContext _userDbContext;

        public ArrangementRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<ArrangementData?>> GetArrangementForUserAsync(long userId)
        {
            var arrangementData = await _userDbContext.Arrangements.Where(data => data.Id == userId).ToListAsync();

            return arrangementData;
        }

        public async Task AddArrangementAsync(ArrangementData arrangementData)
        {
            await _userDbContext.Arrangements.AddAsync(arrangementData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}