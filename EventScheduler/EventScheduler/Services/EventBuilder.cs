using EventScheduler.Models;

namespace EventScheduler.Services
{
    public class EventBuilder
    {
        private readonly Configuration configuration;

        public EventBuilder(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public Event Build(string name, string description, string location, DateTime dateTime)
        {
            var @event = new Event { CreationTime = DateTime.UtcNow };
            UpdateEvent(@event, name, description, location, dateTime);
            return @event;
        }

        public void UpdateEvent(Event @event, string? name = null, string? description = null, string? location = null, DateTime? dateTime = null)
        {
            @event.Name = name ?? @event.Name;
            @event.Location = location ?? @event.Location;
            @event.Date = dateTime ?? @event.Date;
            @event.Description = description ?? @event.Description;
            @event.ReminderTime = @event.Date - configuration.NotificationTime;
        }
    }
}
