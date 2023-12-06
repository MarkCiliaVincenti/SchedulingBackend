using EventScheduler.Models;

namespace EventScheduler.Services
{
    /// <summary>
    /// Sends notifications about events to all the users.
    /// Needs to be extended to support any updates.
    /// </summary>
    public class EventNotifierService
    {
        /// <summary>
        /// Stub implementation for now.
        /// </summary>
        /// <param name="event"></param>
        public void SendNotification(Event @event)
        {
            Console.WriteLine($"Notified event {@event.Id}");
        }
    }
}
