using EventScheduler.Events;
using EventScheduler.Interfaces;
using EventScheduler.Models;
using EventScheduler.Postgres;
using EventScheduler.Services;
using EventScheduler.Utils;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rateLimitingOptions = new RateLimitingOptions();
builder.Configuration.GetSection("RateLimiting").Bind(rateLimitingOptions);

builder.Services.AddRateLimiter(_ => _.AddConcurrencyLimiter(policyName: "trafficLimit", options =>
{
    options.PermitLimit = rateLimitingOptions.PermitLimit;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.QueueLimit = rateLimitingOptions.QueueLimit;
}));

var config = new Configuration();
builder.Configuration.GetSection("Configuration").Bind(config);
builder.Services.AddSingleton(config);

var databaseEvents = new DatabaseEvents();
builder.Services.AddSingleton<IDatabaseEvents>(databaseEvents);
builder.Services.AddSingleton<IDatabaseEventsDistributer>(databaseEvents);

builder.Services.AddSingleton<EventBuilder>();
builder.Services.AddSingleton<PostgresConfiguration>();
builder.Services.AddDbContext<EventsContext>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddSingleton<EventNotifierService>();
builder.Services.AddSingleton<INotificationSchedulerService, NotificationSchedulerService>();
builder.Services.AddHostedService<NotificationTimerService>();

var app = builder.Build();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
