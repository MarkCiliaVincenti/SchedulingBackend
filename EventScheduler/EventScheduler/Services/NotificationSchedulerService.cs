using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Services
{
    public class NotificationSchedulerService(IServiceProvider serviceProvider, Configuration configuration, EventNotifierService eventNotifier) : INotificationSchedulerService
    {
        private readonly Dictionary<int, CancellationToken> notificationTasks = [];

        public async Task ScheduleNotifications()
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            var eventsToNotify = await databaseService.GetEvents(DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.Add(configuration.NotificationServiceRecurrence), false);

            foreach (var @event in eventsToNotify)
            {
                if (!notificationTasks.ContainsKey(@event.Id))
                {
                    var cancellation = new CancellationToken();

                    _ = Notify(@event, cancellation);

                    notificationTasks.Add(@event.Id, cancellation);
                }
            }
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
    }
}
