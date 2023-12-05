using EventScheduler.Interfaces;
using EventScheduler.Models;
using EventScheduler.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Services
{
    public class DatabaseService(EventsContext dbContext, EventBuilder eventBuilder) : IDatabaseService
    {
        public async Task DeleteEvent(string name)
        {
            var @event = await GetEvent(name);

            if(@event != null)
            {
                dbContext.Events.Remove(@event);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEventById(int id)
        {
            dbContext.Events.Remove(new Event { Id = id });
            await dbContext.SaveChangesAsync();
        }

        public async Task<Event?> GetEvent(string name)
        {
            return await dbContext.Events.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Event?> GetEvent(int id)
        {
            return await dbContext.Events.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Event?> GetEventByVenue(string location)
        {
            return await dbContext.Events.Where(x => x.Location == location).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Event>> GetEvents(SortEventsBy sort = SortEventsBy.None, bool isAscending = false)
        {
            switch (sort)
            {
                case SortEventsBy.Popularity:
                    if (isAscending)
                        return await dbContext.Events.OrderBy(e => e.Subscriptions.Count).ToListAsync();
                    else
                        return await dbContext.Events.OrderByDescending(e => e.Subscriptions.Count).ToListAsync();

                case SortEventsBy.CreationTime:
                    if (isAscending)
                        return await dbContext.Events.OrderBy(e => e.CreationTime).ToListAsync();
                    else
                        return await dbContext.Events.OrderByDescending(e => e.CreationTime).ToListAsync();

                case SortEventsBy.Date:
                    if (isAscending)
                        return await dbContext.Events.OrderBy(e => e.Date).ToListAsync();
                    else
                        return await dbContext.Events.OrderByDescending(e => e.Date).ToListAsync();

                case SortEventsBy.None:
                default:
                    return await dbContext.Events.ToListAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEvents(DateTime from, DateTime to, bool isNotified)
        {
            var events = await dbContext.Events.ToListAsync();
            return await dbContext.Events.Where(e => e.Reminded == isNotified && e.ReminderTime > from &&  e.ReminderTime < to).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByLocation(string location)
        {
            return await dbContext.Events.Where(e => e.Location == location).ToListAsync();
        }

        public async Task ScheduleEvent(string name, string description, string location, DateTime dateTime)
        {
            var @event = eventBuilder.Build(name, description, location, dateTime);

            await dbContext.Events.AddAsync(@event);
            await dbContext.SaveChangesAsync();
        }

        public async Task SubscribeToEvent(int id, string email)
        {
            var @event = await GetEvent(id);

            if(@event != null)
            {
                var sub = new Subscription
                {
                    Email = email,
                    EventId = id
                };

                await dbContext.Subscriptions.AddAsync(sub);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateEvent(int id, string? name = null, string? description = null, string? location = null, DateTime? dateTime = null, bool? isReminded = null)
        {
            var @event = await GetEvent(id);

            if(@event != null)
            {
                eventBuilder.UpdateEvent(@event, name, description, location, dateTime, isReminded);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
