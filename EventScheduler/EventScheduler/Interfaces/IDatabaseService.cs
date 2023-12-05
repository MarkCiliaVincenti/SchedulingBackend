using EventScheduler.Models;

namespace EventScheduler.Interfaces
{
    public interface IDatabaseService
    {
        Task<Event> GetEvent(string name);
        Task<Event> GetEvent(int id);
        Task<Event> GetEventByVenue(string location);
        Task<IEnumerable<Event>> GetEvents(SortEventsBy sort = SortEventsBy.None, bool isAscending = false);
        Task<IEnumerable<Event>> GetEvents(DateTime from, DateTime to, bool isNotified);
        Task<IEnumerable<Event>> GetEventsByLocation(string location);
        Task<Event> ScheduleEvent(string name, string description, string location, DateTime dateTime);
        Task<Event> UpdateEvent(int id, string? name = null, string? description = null, string? location = null, DateTime? dateTime = null, bool? isReminded = null);
        Task<Event> DeleteEvent(string name);
        Task<Event> DeleteEventById(int id);
        Task SubscribeToEvent(int id, string email);
    }
}
