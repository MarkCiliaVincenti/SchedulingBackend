using EventScheduler.Models;
using static EventScheduler.Events.DatabaseEvents;

namespace EventScheduler.Interfaces
{
    public interface IDatabaseEvents
    {
        event EventHandler<Event> EventUpdated;
        event EventHandler<Event> EventDeleted;
        event EventHandler<Event> EventCreated;
    }
}
