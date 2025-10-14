using BotWhatsapp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ClientSession> ClientSessions => Set<ClientSession>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ResourceService> ResourceServices => Set<ResourceService>();
    public DbSet<Service> Services { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<ReminderRecipient> ReminderRecipients { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Calendar> Calendars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResourceService>()
            .HasKey(es => new { es.ResourceId, es.ServiceId });

        modelBuilder.Entity<ResourceService>()
            .HasOne(es => es.Service)
            .WithMany(s => s.ResourceServices)
            .HasForeignKey(es => es.ServiceId);


        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Client)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AssignedResource)
            .WithMany()
            .HasForeignKey(a => a.AssignedResourceId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AssignedResource)
            .WithMany()
            .HasForeignKey(a => a.AssignedResourceId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Calendar>()
            .HasOne<Resource>()
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Event>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Event>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Event>()
            .Property(e => e.AppointmentCode)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Event>()
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        
    }
}
