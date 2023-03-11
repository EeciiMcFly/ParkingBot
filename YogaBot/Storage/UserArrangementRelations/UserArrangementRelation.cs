using System.ComponentModel.DataAnnotations;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Users;

namespace YogaBot.Storage.UserArrangementRelations
{
    public class UserArrangementRelation
    {
        [Key]
        public long UserArrangementRelationId { get; set; }

        public long UserId { get; set; }

        public long ArrangementId { get; set; }

        public Role Role { get; set; }

        public virtual User User { get; set; }

        public virtual Arrangement Arrangement { get; set; }
    }
}