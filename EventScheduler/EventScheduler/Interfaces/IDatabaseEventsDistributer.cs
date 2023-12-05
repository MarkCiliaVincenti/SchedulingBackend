using EventScheduler.Models;

namespace EventScheduler.Interfaces
{
    public interface IDatabaseEventsDistributer
    {
        void DisbributeEventCreated(Event e);
        void DistributeEventDeleted(Event e);
        void DistributeEventUpdated(Event e);
    }
}
