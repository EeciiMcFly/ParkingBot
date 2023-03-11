using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.UserArrangementRelations
{
    public interface IUserArrangementRelationsRepository
    {
        Task<IEnumerable<UserArrangementRelation?>> GetRelationsForUserAsync(long userId);
        
        Task<IEnumerable<UserArrangementRelation?>> GetRelationsForArrangementAsync(long arrangementId);

        Task AddUserEventRelationAsync(UserArrangementRelation userArrangementRelation);

        Task DeleteRelationByArrangementId(long arrangementId);

        Task DeleteRelationByArrangementIdAndUserId(long arrangementId, long userId);
    }
    
    public class UserArrangementRelationsRepository : IUserArrangementRelationsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public UserArrangementRelationsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<UserArrangementRelation?>> GetRelationsForUserAsync(long userId)
        {
            var relationsData = await _userDbContext.UserEventRelations.Where(data => data.UserId == userId).ToListAsync();

            return relationsData;
        }
        
        public async Task<IEnumerable<UserArrangementRelation?>> GetRelationsForArrangementAsync(long arrangementId)
        {
            var relationsData = await _userDbContext.UserEventRelations.Where(data => data.ArrangementId == arrangementId).ToListAsync();

            return relationsData;
        }

        public async Task AddUserEventRelationAsync(UserArrangementRelation userArrangementRelation)
        {
            await _userDbContext.UserEventRelations.AddAsync(userArrangementRelation);

            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeleteRelationByArrangementId(long arrangementId)
        {
            var userArrangementRelations = await _userDbContext.UserEventRelations.Where(data => data.ArrangementId == arrangementId).ToListAsync();
            _userDbContext.RemoveRange(userArrangementRelations);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeleteRelationByArrangementIdAndUserId(long arrangementId, long userId)
        {
            var userArrangementRelations = await _userDbContext.UserEventRelations.FirstOrDefaultAsync(data => data.UserId == userId && data.ArrangementId == arrangementId);
            _userDbContext.Remove(userArrangementRelations);
            await _userDbContext.SaveChangesAsync();
        }
    }
}