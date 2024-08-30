using Microsoft.EntityFrameworkCore;
using Subscription.Contract.Models;

namespace FireAndForget.Notification.Job.Data;
public class NotificationContext : DbContext
{
    public NotificationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<SubscriptionNotification> SubscriptionNotifications { get; set; }
}
