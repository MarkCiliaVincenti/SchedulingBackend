namespace EventScheduler.Models
{
    public class RateLimitingOptions
    {
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
    }
}
