using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventScheduler.Models
{
    [Table("event_subscription")]
    public class Subscription
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column ("event_id")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}
