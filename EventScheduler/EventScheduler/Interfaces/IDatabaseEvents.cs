using EventScheduler.Models;
using static EventScheduler.Events.DatabaseEvents;

namespace EventScheduler.Interfaces
{
    /// <summary>
    /// All possible events that influence an event.
    /// </summary>
    public interface IDatabaseEvents
    {
        event EventHandler<Event> EventUpdated;
        event EventHandler<Event> EventDeleted;
        event EventHandler<Event> EventCreated;
    }
}
