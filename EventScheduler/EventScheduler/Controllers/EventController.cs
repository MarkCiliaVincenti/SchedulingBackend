using EventScheduler.Interfaces;
using EventScheduler.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventScheduler.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class EventController(IDatabaseService databaseService) : ControllerBase
    {
        private readonly IDatabaseService databaseService = databaseService;

        [HttpPost(Name = "ScheduleEvent")]
        public async Task ScheduleEvent(string name, string description, string location, DateTime dateTime)
        {
            await databaseService.ScheduleEvent(name, description, location, dateTime.ToUniversalTime());
        }

        [HttpGet(Name = "AllEvents")]
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await databaseService.GetEvents();
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
            await databaseService.UpdateEvent(id, name, description, location, dateTime);
        }

        [HttpDelete(Name = "DeleteEventByName")]
        public async Task DeleteEventByName(string name)
        {
            await databaseService.DeleteEvent(name);
        }

        [HttpDelete(Name = "DeleteEventById")]
        public async Task DeleteEventById(int id)
        {
            await databaseService.DeleteEventById(id);
        }

        [HttpPost(Name = "Subscribe")]
        public async Task SubscribeToEvent(int id, string email)
        {
            await databaseService.SubscribeToEvent(id, email);
        }

        public async Task<IEnumerable<Event>> GetEventsByLocation(string location)
        {
            return await databaseService.GetEventsByLocation(location);
        }
    }
}
