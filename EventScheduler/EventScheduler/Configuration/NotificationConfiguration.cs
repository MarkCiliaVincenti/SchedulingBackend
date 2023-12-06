namespace EventScheduler.Configuration
{
    /// <summary>
    /// General configuration for the system.
    /// </summary>
    public class NotificationConfiguration
    {
        public TimeSpan NotificationTime { get; set; }
        public TimeSpan NotificationServiceRecurrence { get; set; }
    }
}
