using EventScheduler.Interfaces;
using EventScheduler.Models;
using EventScheduler.Postgres;
using EventScheduler.Services;
using EventScheduler.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new Configuration();
builder.Configuration.GetSection("Configuration").Bind(config);
builder.Services.AddSingleton(config);

builder.Services.AddSingleton<EventBuilder>();
builder.Services.AddSingleton<PostgresConfiguration>();
builder.Services.AddDbContext<EventsContext>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddSingleton<EventNotifierService>();
builder.Services.AddSingleton<INotificationSchedulerService, NotificationSchedulerService>();
builder.Services.AddHostedService<NotificationTimerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
