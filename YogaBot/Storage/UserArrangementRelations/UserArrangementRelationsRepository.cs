using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.UserArrangementRelations
{
    public interface IUserEventRelationsRepository
    {
        Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForUserAsync(long userId);
        
        Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForArrangementAsync(long arrangementId);

        Task AddUserEventRelationAsync(UserArrangementRelationData userArrangementRelationData);
    }
    
    public class UserArrangementRelationsRepository : IUserEventRelationsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public UserArrangementRelationsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForUserAsync(long userId)
        {
            var relationsData = await _userDbContext.UserEventRelations.Where(data => data.UserId == userId).ToListAsync();

            return relationsData;
        }
        
        public async Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForArrangementAsync(long arrangementId)
        {
            var relationsData = await _userDbContext.UserEventRelations.Where(data => data.ArrangementId == arrangementId).ToListAsync();

            return relationsData;
        }

        public async Task AddUserEventRelationAsync(UserArrangementRelationData userArrangementRelationData)
        {
            await _userDbContext.UserEventRelations.AddAsync(userArrangementRelationData);

            await _userDbContext.SaveChangesAsync();
        }
    }
}