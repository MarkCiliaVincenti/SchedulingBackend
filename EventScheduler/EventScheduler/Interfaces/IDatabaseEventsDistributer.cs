using EventScheduler.Models;

namespace EventScheduler.Interfaces
{
    /// <summary>
    /// This infterface has all the methods to distribute updates about events.
    /// </summary>
    public interface IDatabaseEventsDistributer
    {
        void DisbributeEventCreated(Event e);
        void DistributeEventDeleted(Event e);
        void DistributeEventUpdated(Event e);
    }
}
