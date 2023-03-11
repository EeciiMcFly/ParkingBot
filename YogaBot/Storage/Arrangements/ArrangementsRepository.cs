using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Arrangements
{
    public interface IArrangementRepository
    {
        Task<Arrangement?> GetArrangementAsync(long arrangementId);

        Task AddArrangementAsync(Arrangement user);
    }
    
    public class ArrangementRepository : IArrangementRepository
    {
        private readonly StorageDbContext _userDbContext;

        public ArrangementRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Arrangement?> GetArrangementAsync(long arrangementId)
        {
            var arrangementData = await _userDbContext.Arrangements.FirstOrDefaultAsync(data => data.ArrangementId == arrangementId);

            return arrangementData;
        }

        public async Task AddArrangementAsync(Arrangement arrangement)
        {
            await _userDbContext.Arrangements.AddAsync(arrangement);

            await _userDbContext.SaveChangesAsync();
        }
    }
}