using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Events
{
    /// <summary>
    /// Events need to be tracked because they can be updated and used across the whole system.
    /// This class provides all of the events(c#) about any update to events(database).
    /// </summary>
    public class DatabaseEvents : IDatabaseEvents, IDatabaseEventsDistributer
    {
        public event EventHandler<Event> EventUpdated;

        public event EventHandler<Event> EventDeleted;

        public event EventHandler<Event> EventCreated;

        public void DisbributeEventCreated(Event e)
        {
            EventCreated?.Invoke(this, e);
        }

        public void DistributeEventDeleted(Event e)
        {
            EventDeleted?.Invoke(this, e);
        }

        public void DistributeEventUpdated(Event e)
        {
            EventUpdated?.Invoke(this, e);
        }
    }
}
