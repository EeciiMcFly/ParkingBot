namespace YogaBot.Storage.Events
{
    public class EventData
    {
        public long Id { get; set; }

        public long ArrangementId { get; set; }

        public DateTime Date { get; set; }
        
        public string Name { get; set; }
        
        public int Cost { get; set; }
    }
}