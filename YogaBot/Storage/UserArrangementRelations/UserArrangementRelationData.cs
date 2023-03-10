namespace YogaBot.Storage.UserArrangementRelations
{
    public class UserArrangementRelationData
    {
        public long Id { get; set; }
        
        public long UserId { get; set; }

        public long ArrangementId { get; set; }

        public Role Role { get; set; }
    }
}