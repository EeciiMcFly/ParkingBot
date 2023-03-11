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

        public bool PollSend { get; set; }

        public long? PollMessageId { get; set; }
        public string? PollId { get; set; }

        public virtual Arrangement Arrangement { get; set; }
    }
}