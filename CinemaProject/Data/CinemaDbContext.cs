// Data/CinemaDbContext.cs
using Microsoft.EntityFrameworkCore;
using CinemaProject.Models;

namespace CinemaBackend.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<GiftCardTransaction> GiftCardTransactions { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatReservation> SeatReservations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure Ad uses its Id as primary key, and match the varchar(500) length in the database
            modelBuilder.Entity<Ad>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Title)
                      .HasColumnType("varchar(200)")
                      .HasMaxLength(200);

                entity.Property(a => a.ClientName)
                      .HasColumnType("varchar(200)")
                      .HasMaxLength(200);

                entity.Property(a => a.MediaUrl)
                      .HasColumnType("varchar(500)")
                      .HasMaxLength(500);

                entity.Property(a => a.Placement)
                      .HasColumnType("varchar(500)")
                      .HasMaxLength(500);

                entity.Property(a => a.Status)
                      .HasColumnType("varchar(50)")
                      .HasMaxLength(50);
            });

            // You can add further modelBuilder.Entity<OtherModel>() calls here if you need
            // to configure table names, relationships, etc.
        }
    }
}
//saaa