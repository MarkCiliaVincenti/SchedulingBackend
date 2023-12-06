
using EventScheduler.Configuration;
using EventScheduler.Interfaces;

namespace EventScheduler.Services
{
    public class NotificationTimerService(NotificationConfiguration configuration, INotificationSchedulerService notificationSchedulerService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Schedule once before waiting to not miss anything.
            using var timer = new PeriodicTimer(configuration.NotificationServiceRecurrence);
            do
            {
                await notificationSchedulerService.ScheduleNotifications();

            } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
