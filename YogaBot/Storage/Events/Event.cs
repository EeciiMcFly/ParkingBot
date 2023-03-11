using System.ComponentModel.DataAnnotations;
using YogaBot.Storage.Arrangements;

namespace YogaBot.Storage.Events
{
    public class Event
    {
        [Key]
        public long EventId { get; set; }

        public long ArrangementId { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public virtual Arrangement Arrangement { get; set; }
    }
}