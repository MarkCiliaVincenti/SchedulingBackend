using AsyncKeyedLock;
using EventScheduler.Configuration;
using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Services
{
    /// <summary>
    /// This class schedules tasks to remind subscribers of events about events.
    /// It also tracks all updates about events to never miss notifications.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="configuration"></param>
    /// <param name="eventNotifier"></param>
    public class NotificationSchedulerService(IServiceProvider serviceProvider,
                                              NotificationConfiguration configuration,
                                              EventNotifierService eventNotifier) : INotificationSchedulerService
    {
        // Contains current tasks that are waiting to notify.
        private readonly Dictionary<int, CancellationTokenSource> notificationTasks = [];

        // Lock on id instead of lock object to allow better concurrency.
        private readonly AsyncKeyedLocker<int> asyncKeyedLocker = new();


        public NotificationSchedulerService(IServiceProvider serviceProvider,
                                            NotificationConfiguration configuration,
                                            EventNotifierService eventNotifier,
                                            IDatabaseEvents databaseEvents) : this(serviceProvider, configuration, eventNotifier)
        {
            databaseEvents.EventUpdated += OnEventUpdated;
            databaseEvents.EventDeleted += OnEventDeleted;
            databaseEvents.EventCreated += OnEventCreated;
        }

        /// <summary>
        /// Schedules all events in current timeframe to be notified.
        /// </summary>
        /// <returns></returns>
        public async Task ScheduleNotifications()
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            // Gets events that have not been notified yet with a bit of overhead to not miss any.
            var eventsToNotify = await databaseService.GetEvents(DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.Add(configuration.NotificationServiceRecurrence), false);

            foreach (var @event in eventsToNotify)
            {
                if (!notificationTasks.ContainsKey(@event.Id))
                    _ = ScheduleNotification(@event);
            }
        }

        private async Task ScheduleNotification(Event @event)
        {
            // Allow task to be canceled in case the event has been updated or deleted.
            var cancellation = new CancellationTokenSource();

            using (await asyncKeyedLocker.LockAsync(@event.Id))
            notificationTasks.Add(@event.Id, cancellation);

            _ = Notify(@event, cancellation.Token);
        }

        private async Task Notify(Event @event, CancellationToken cancellationToken)
        {
            // Wait for the exact time for the reminder.
            await Task.Delay(@event.ReminderTime - DateTime.UtcNow, cancellationToken);

            eventNotifier.SendNotification(@event);

            using (await asyncKeyedLocker.LockAsync(@event.Id))
            notificationTasks.Remove(@event.Id);

            using IServiceScope scope = serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
            await databaseService.UpdateEvent(@event.Id, isReminded: true);
        }

        private async void OnEventCreated(object? _, Event e)
        {
            using (await asyncKeyedLocker.LockAsync(e.Id))
                CheckEventToSchedule(e);
        }

        private async void OnEventDeleted(object? _, Event e)
        {
            using (await asyncKeyedLocker.LockAsync(e.Id))
                RemoveScheduledNotification(e);
        }

        private async void OnEventUpdated(object? _, Event e)
        {
            using (await asyncKeyedLocker.LockAsync(e.Id))
            {
                RemoveScheduledNotification(e);

                CheckEventToSchedule(e);
            }
        }

        private void RemoveScheduledNotification(Event e)
        {
            if (notificationTasks.TryGetValue(e.Id, out CancellationTokenSource? value))
            {
                value.Cancel();
                notificationTasks.Remove(e.Id);
            }
        }

        private void CheckEventToSchedule(Event e)
        {
            if (e.ReminderTime - DateTime.UtcNow < configuration.NotificationServiceRecurrence)
                _ = ScheduleNotification(e);
        }
    }
}
