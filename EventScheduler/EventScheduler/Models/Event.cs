using System.ComponentModel.DataAnnotations.Schema;

namespace EventScheduler.Models
{
    [Table("scheduled_events")]
    public class Event
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("reminder_time")]
        public DateTime ReminderTime {  get; set; }

        [Column("reminded")]
        public bool Reminded { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
