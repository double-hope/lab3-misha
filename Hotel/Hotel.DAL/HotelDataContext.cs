using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Hotel.DAL.Entities;

namespace Hotel.DAL
{
    public class HotelDataContext : DbContext
    {
        public HotelDataContext() { }
        public HotelDataContext(DbContextOptions options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomCategory> RoomCategories { get; set; }
        public DbSet<RoomRental> RoomRentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasKey(rc => rc.Id);

            modelBuilder.Entity<RoomCategory>()
                .HasKey(rc => rc.Id);

            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RoomRental>()
                .HasKey(rr => rr.Id);

            modelBuilder.Entity<Room>()
               .HasOne(r => r.Category)
               .WithMany()
               .HasForeignKey(r => r.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomRental>()
                .HasOne(rr => rr.Room)
                .WithMany(r => r.RoomRentals)
                .HasForeignKey(rr => rr.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
