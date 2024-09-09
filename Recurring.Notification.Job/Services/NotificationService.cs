using Microsoft.EntityFrameworkCore;
using Subscription.Contract.Data;
using Subscription.Contract.Models;

namespace Recurring.Notification.Job.Services;
public class NotificationService
{
    private readonly NotificationContext _context;

    public NotificationService(NotificationContext context)
    {
        _context = context;
    }

    public async Task SendExpirationNotificationsAsync()
    {
        try
        {
            var subscribersToNotify = await _context.Subscribers
                .Where(s => s.ExpirationDate <= DateTime.Now.AddDays(7))
                .ToListAsync();

            if (!subscribersToNotify.Any())
            {
                Console.WriteLine("Nothing to process");
                return;
            }

            var subscriptionNotifications = new List<SubscriptionNotification>();

            foreach (var subscriber in subscribersToNotify)
            {
                var emailBody = $@"<html>
                                    <body>
                                        <h1>Subscription Expiration Notice</h1>
                                        <p>Dear {subscriber.Name},</p>
                                        <p>This is a reminder that your subscription will expire on <strong>{subscriber.ExpirationDate:MMMM dd, yyyy}</strong>.</p>
                                        <p>If you wish to continue enjoying our services, please renew your subscription before the expiration date.</p>
                                        <p>Thank you for your continued support!</p>
                                        <p>Sincerely, NotifyMe</p>
                                    </body>
                                    </html>";

                subscriptionNotifications.Add(new SubscriptionNotification(Guid.NewGuid(), subscriber.Name, subscriber.Email, emailBody, subscriber.ExpirationDate));
            }

            await _context.SubscriptionNotifications.AddRangeAsync(subscriptionNotifications);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
