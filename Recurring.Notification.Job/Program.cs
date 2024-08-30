using Hangfire;
using Microsoft.EntityFrameworkCore;
using Recurring.Notification.Job.Data;
using Recurring.Notification.Job.Services;
using Subscription.Contract.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotificationContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

app.UseHangfireDashboard();
app.UseHangfireServer();

var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<NotificationService>("SendExpirationNotifications", service =>
    service.SendExpirationNotificationsAsync(), Cron.Minutely);

app.MapPost("/subscriptions/subscriber", async (Subscriber request, NotificationContext context) =>
{
    var subscriber = new Subscriber(Guid.NewGuid(), request.Name, request.Email, request.ExpirationDate, false);

    context.Subscribers.Add(subscriber);
    await context.SaveChangesAsync();

    return Results.Created($"/subscriptions/subscriber/{subscriber.Id}", subscriber);
});

app.Run();
