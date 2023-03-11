using System.ComponentModel.DataAnnotations;

namespace YogaBot.Storage.Arrangements
{
    public class Arrangement
    {
        [Key]
        public long ArrangementId { get; set; }

        public string Name { get; set; }

        public long ChatId { get; set; }
    }
}