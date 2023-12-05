using EventScheduler.Interfaces;
using EventScheduler.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventScheduler.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class EventController(IDatabaseService databaseService, IDatabaseEventsDistributer databaseEventsDistributer) : ControllerBase
    {
        private readonly IDatabaseService databaseService = databaseService;

        [HttpPost(Name = "ScheduleEvent")]
        public async Task ScheduleEvent(string name, string description, string location, DateTime dateTime)
        {
            var @event = await databaseService.ScheduleEvent(name, description, location, dateTime.ToUniversalTime());

            databaseEventsDistributer.DisbributeEventCreated(@event);
        }

        [HttpGet(Name = "AllEvents")]
        public async Task<IEnumerable<Event>> GetAllEvents(SortEventsBy sort = SortEventsBy.None, bool isAscending = true)
        {
            return await databaseService.GetEvents(sort, isAscending);
        }

        [HttpGet(Name = "EventByName")]
        public async Task<Event> GetEventByName(string name)
        {
            return await databaseService.GetEvent(name);
        }

        [HttpGet(Name = "EventById")]
        public async Task<Event> GetEventById(int id)
        {
            return await databaseService.GetEvent(id);
        }

        [HttpPost(Name = "UpdateEvent")]
        public async Task UpdateEvent(int id, string? name = null, string? description = null, string? location = null, DateTime? dateTime = null)
        {
            var @event = await databaseService.UpdateEvent(id, name, description, location, dateTime);

            databaseEventsDistributer.DistributeEventUpdated(@event);
        }

        [HttpDelete(Name = "DeleteEventByName")]
        public async Task DeleteEventByName(string name)
        {
            var @event = await databaseService.DeleteEvent(name);

            databaseEventsDistributer.DistributeEventDeleted(@event);
        }

        [HttpDelete(Name = "DeleteEventById")]
        public async Task DeleteEventById(int id)
        {
            var @event = await databaseService.DeleteEventById(id);

            databaseEventsDistributer.DistributeEventDeleted(@event);
        }

        [HttpPost(Name = "Subscribe")]
        public async Task SubscribeToEvent(int id, string email)
        {
            await databaseService.SubscribeToEvent(id, email);
        }

        [HttpGet(Name = "EventsByLocation")]
        public async Task<IEnumerable<Event>> GetEventsByLocation(string location)
        {
            return await databaseService.GetEventsByLocation(location);
        }
    }
}
