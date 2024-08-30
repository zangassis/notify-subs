using Delayed.Notification.Job.Data;
using Microsoft.EntityFrameworkCore;
using Subscription.Contract.Models;

namespace Delayed.Notification.Job.Services;
public class DelayedService
{
    private readonly NotificationContext _context;

    public DelayedService(NotificationContext context)
    {
        _context = context;
    }

    public async Task SendFinalExpirationReminderAsync()
    {
        var subscribersToNotify = await _context.Subscribers
            .Where(s => s.ExpirationDate <= DateTime.Now.AddDays(1))
            .ToListAsync();

        if (!subscribersToNotify.Any())
        {
            Console.WriteLine("Nothing to process");
            return;
        }

        foreach (var subscriber in subscribersToNotify)
        {
            try
            {
                var emailBody = $@"<html>
                                   <body>
                                       <h1>Final Reminder: Subscription Expiration</h1>
                                       <p>Dear {subscriber.Name},</p>
                                       <p>Your subscription is set to expire tomorrow on <strong>{subscriber.ExpirationDate:MMMM dd, yyyy}</strong>.</p>
                                       <p>Please renew your subscription to avoid interruption of service.</p>
                                       <p>Thank you for your continued support!</p>
                                       <p>Sincerely, NotifyMe</p>
                                   </body>
                                   </html>";

                var notification = new SubscriptionNotification(Guid.NewGuid(), subscriber.Name, subscriber.Email, emailBody, subscriber.ExpirationDate);

                await _context.SubscriptionNotifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}