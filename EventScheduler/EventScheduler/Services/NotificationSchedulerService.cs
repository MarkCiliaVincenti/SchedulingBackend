using AsyncKeyedLock;
using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Services
{
    public class NotificationSchedulerService(IServiceProvider serviceProvider,
                                              Configuration configuration,
                                              EventNotifierService eventNotifier) : INotificationSchedulerService
    {
        private readonly Dictionary<int, CancellationTokenSource> notificationTasks = [];
        private readonly AsyncKeyedLocker<int> asyncKeyedLocker = new();


        public NotificationSchedulerService(IServiceProvider serviceProvider,
                                            Configuration configuration,
                                            EventNotifierService eventNotifier,
                                            IDatabaseEvents databaseEvents) : this(serviceProvider, configuration, eventNotifier)
        {
            databaseEvents.EventUpdated += OnEventUpdated;
            databaseEvents.EventDeleted += OnEventDeleted;
            databaseEvents.EventCreated += OnEventCreated;
        }

        public async Task ScheduleNotifications()
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            var eventsToNotify = await databaseService.GetEvents(DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.Add(configuration.NotificationServiceRecurrence), false);

            foreach (var @event in eventsToNotify)
            {
                using (await asyncKeyedLocker.LockAsync(@event.Id))
                    if (!notificationTasks.ContainsKey(@event.Id))
                    {
                        ScheduleNotification(@event);
                    }
            }
        }

        private void ScheduleNotification(Event @event)
        {
            var cancellation = new CancellationTokenSource();

            _ = Notify(@event, cancellation.Token);

            notificationTasks.Add(@event.Id, cancellation);
        }

        private async Task Notify(Event @event, CancellationToken cancellationToken)
        {
            await Task.Delay(@event.ReminderTime - DateTime.UtcNow, cancellationToken);

            eventNotifier.SendNotification(@event);

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
                ScheduleNotification(e);
        }
    }
}
