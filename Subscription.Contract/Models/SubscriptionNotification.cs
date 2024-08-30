namespace Subscription.Contract.Models;
public record SubscriptionNotification(Guid Id, string SubscriberName, string SubscriberEmail, string EmailBody, DateTime ExpirationDate);
