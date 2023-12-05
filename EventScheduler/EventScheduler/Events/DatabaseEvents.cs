using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Events
{
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
