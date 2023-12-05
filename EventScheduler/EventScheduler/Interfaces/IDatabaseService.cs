using EventScheduler.Models;

namespace EventScheduler.Interfaces
{
    public interface IDatabaseService
    {
        Task<Event> GetEvent(string name);
        Task<Event> GetEvent(int id);
        Task<Event> GetEventByVenue(string location);
        Task<IEnumerable<Event>> GetEvents();
        Task ScheduleEvent(string name, string description, string location, DateTime dateTime);
        Task UpdateEvent(int id, string? name = null, string? description = null, string? location = null, DateTime? dateTime = null, bool? isReminded = null);
        Task DeleteEvent(string name);
        Task DeleteEventById(int id);
        Task SubscribeToEvent(int id, string email);
        Task<IEnumerable<Event>> GetEventsByLocation(string location);
    }
}
