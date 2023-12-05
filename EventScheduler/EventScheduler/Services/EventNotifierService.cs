using EventScheduler.Models;

namespace EventScheduler.Services
{
    public class EventNotifierService
    {
        public void SendNotification(Event @event)
        {
            Console.WriteLine($"Notified event {@event.Id}");
        }
    }
}
