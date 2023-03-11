using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Arrangements
{
    public interface IArrangementRepository
    {
        Task<Arrangement?> GetArrangementAsync(long arrangementId);
        
        Task<Arrangement?> GetArrangementByChatIdAsync(long chatId);

        Task<Arrangement> AddArrangementAsync(Arrangement user);

        Task DeleteArrangementAsync(Arrangement arrangement);
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

        public async Task<Arrangement?> GetArrangementByChatIdAsync(long chatId)
        {
            var arrangementData = await _userDbContext.Arrangements.FirstOrDefaultAsync(data => data.ChatId == chatId);

            return arrangementData;
        }

        public async Task<Arrangement> AddArrangementAsync(Arrangement arrangement)
        {
            var entityEntry = await _userDbContext.Arrangements.AddAsync(arrangement);

            await _userDbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task DeleteArrangementAsync(Arrangement arrangement)
        {
            _userDbContext.Arrangements.Remove(arrangement);

            await _userDbContext.SaveChangesAsync();
        }
    }
}