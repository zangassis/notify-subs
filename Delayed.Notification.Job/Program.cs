using Delayed.Notification.Job.Data;
using Delayed.Notification.Job.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NotificationContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddTransient<DelayedService>();

var app = builder.Build();

app.UseHangfireDashboard();

var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();

backgroundJobClient.Schedule((DelayedService service) => service.SendFinalExpirationReminderAsync(), TimeSpan.FromMinutes(2));

app.Run();