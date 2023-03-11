using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.UserArrangementRelations
{
    public interface IUserArrangementRelationsRepository
    {
        Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForUserAsync(Guid userId);
        
        Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForArrangementAsync(Guid arrangementId);

        Task AddUserEventRelationAsync(UserArrangementRelationData userArrangementRelationData);
    }
    
    public class UserArrangementRelationsRepository : IUserArrangementRelationsRepository
    {
        private readonly StorageDbContext _userDbContext;

        public UserArrangementRelationsRepository(StorageDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForUserAsync(Guid userId)
        {
            var relationsData = await _userDbContext.UserEventRelations.Where(data => data.UserId == userId).ToListAsync();

            return relationsData;
        }
        
        public async Task<IEnumerable<UserArrangementRelationData?>> GetRelationsForArrangementAsync(Guid arrangementId)
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