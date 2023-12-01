using EventScheduler.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Models
{
    public class EventsContext : DbContext
    {
        private readonly PostgresConfiguration configuration;

        public DbSet<Event> Events { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public EventsContext(PostgresConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                connectionString: $"Server={configuration.Server};Port={configuration.Port};User Id={configuration.User};Password={configuration.Password};Database={configuration.Database};");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Subscriptions)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId)
                .IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
