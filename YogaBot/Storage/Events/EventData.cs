namespace YogaBot.Storage.Events
{
    public class EventData
    {
        public Guid Id { get; set; }

        public Guid ArrangementId { get; set; }

        public DateTime Date { get; set; }
        
        public string Name { get; set; }
        
        public int Cost { get; set; }
    }
}