using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.UserArrangementRelations
{
    public interface IUserArrangementRelationsRepository
    {
        Task<IEnumerable<UserArrangementRelation?>> GetRelationsForUserAsync(long userId);
        
        Task<IEnumerable<UserArrangementRelation?>> GetRelationsForArrangementAsync(long arrangementId);

        Task AddUserEventRelationAsync(UserArrangementRelation userArrangementRelation);
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
    }
}