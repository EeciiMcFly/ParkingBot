namespace YogaBot.Storage.UserArrangementRelations
{
    public class UserArrangementRelationData
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public Guid ArrangementId { get; set; }

        public Role Role { get; set; }
    }
}