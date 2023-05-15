using FlightBooking.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace FlightBooking.Database
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Shifted to Program.cs
            //optionsBuilder.UseInMemoryDatabase(databaseName: "FlightBooking");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Flight>().Property(p => p.Days).HasConversion(new ArrayConverter());
            modelBuilder.Entity<User>().Property(p => p.Role).HasConversion(new RoleConverter());
            modelBuilder.Entity<Booking>().Property(p => p.TripType).HasConversion(new TripTypeConverter());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Flight> Flights { get; set; }
    }

    public class ArrayConverter : ValueConverter<string[], string>
    {
        public ArrayConverter() : base(x => string.Join(";", x), x => x.Split(";", StringSplitOptions.RemoveEmptyEntries)) { }

    }

    public class RoleConverter : ValueConverter<Role, string>
    {
        public RoleConverter() : base(x => x.ToString(), x => (Role)Enum.Parse(typeof(Role), x)) { }
    }

    public class TripTypeConverter : ValueConverter<TripType, string>
    {
        public TripTypeConverter() : base(x => x.ToString(), x => (TripType)Enum.Parse(typeof(TripType), x)) { }
    }
}

