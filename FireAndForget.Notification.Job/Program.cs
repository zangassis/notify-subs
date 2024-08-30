
using FireAndForget.Notification.Job.Data;
using FireAndForget.Notification.Job.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NotificationContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddTransient<FireAndForgetService>();

var app = builder.Build();

app.UseHangfireDashboard();

var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();

backgroundJobClient.Enqueue((FireAndForgetService service) => service.ConfirmSubscriptionRenewalAsync());

app.Run();