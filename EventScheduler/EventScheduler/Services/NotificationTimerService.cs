
using EventScheduler.Interfaces;
using EventScheduler.Models;

namespace EventScheduler.Services
{
    public class NotificationTimerService(Configuration configuration, INotificationSchedulerService notificationSchedulerService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(configuration.NotificationServiceRecurrence);

            do
            {
                await notificationSchedulerService.ScheduleNotifications();

            } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
