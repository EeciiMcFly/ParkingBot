using System.ComponentModel.DataAnnotations;
using YogaBot.Storage.Events;
using YogaBot.Storage.Users;

namespace YogaBot.Storage.Presences
{
    public class Presence
    {
        [Key]
        public long PresenceId { get; set; }

        public long EventId { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public Event Event { get; set; }
    }
}