using Microsoft.EntityFrameworkCore;
using Subscription.Contract.Data;
using Subscription.Contract.Models;

namespace FireAndForget.Notification.Job.Services;
public class FireAndForgetService
{
    private readonly NotificationContext _context;

    public FireAndForgetService(NotificationContext context)
    {
        _context = context;
    }

    public async Task ConfirmSubscriptionRenewalAsync()
    {
        try
        {
            var subscribersToNotify = await _context.Subscribers
                .Where(s => s.SubscriptionRenewed)
                .ToListAsync();

            if (!subscribersToNotify.Any())
            {
                Console.WriteLine("Nothing to process");
                return;
            }

            foreach (var subscriber in subscribersToNotify)
            {
                var newExpirationDate = subscriber.ExpirationDate.AddMonths(1);

                var emailBody = $@"<html>
                                <body>
                                    <h1>Subscription Renewal Confirmation</h1>
                                    <p>Dear {subscriber.Name},</p>
                                    <p>Thank you for renewing your subscription. Your new expiration date is <strong>{newExpirationDate:MMMM dd, yyyy}</strong>.</p>
                                    <p>We appreciate your continued support!</p>
                                    <p>Sincerely, NotifyMe</p>
                                </body>
                                </html>";

                var notification = new SubscriptionNotification(Guid.NewGuid(), subscriber.Name, subscriber.Email, emailBody, newExpirationDate);

                await _context.SubscriptionNotifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
