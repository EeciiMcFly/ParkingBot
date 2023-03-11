using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Arrangements
{
    public interface IArrangementRepository
    {
        Task<ArrangementData?> GetArrangementAsync(Guid arrangementId);

        Task AddArrangementAsync(ArrangementData userData);
    }
    
    public class ArrangementRepository : IArrangementRepository
    {
        private readonly StorageDbContext _userDbContext;

        public ArrangementRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<ArrangementData?> GetArrangementAsync(Guid arrangementId)
        {
            var arrangementData = await _userDbContext.Arrangements.FirstOrDefaultAsync(data => data.Id == arrangementId);

            return arrangementData;
        }

        public async Task AddArrangementAsync(ArrangementData arrangementData)
        {
            await _userDbContext.Arrangements.AddAsync(arrangementData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}