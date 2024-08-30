namespace Subscription.Contract.Models;
public record Subscriber(Guid Id, string Name, string Email, DateTime ExpirationDate, bool SubscriptionRenewed);