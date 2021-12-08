using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UniversalParking.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace UniversalParking.DAL.EF
{
    public class UniversalParkingContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        internal DbSet<User> users { get; set; }
        internal DbSet<Car> cars { get; set; }
        internal DbSet<Parking> parkings { get; set; }
        internal DbSet<ParkingPlace> parkingPlaces { get; set; }
        internal DbSet<Booking> bookings { get; set; }

        public UniversalParkingContext(DbContextOptions<UniversalParkingContext> options) : base(options)
        {           
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<User>()
                 .Property(user => user.UserID)
                 .ValueGeneratedNever();

            builder.Entity<Car>()
                .Property(car => car.CarID)
                .ValueGeneratedOnAdd();

            builder.Entity<Parking>()
                .Property(parking => parking.ParkingID)
                .ValueGeneratedOnAdd();

            builder.Entity<ParkingPlace>()
                .Property(parkingPlace => parkingPlace.ParkingPlaceID)
                .ValueGeneratedOnAdd();

            builder.Entity<Booking>()
                .Property(booking => booking.BookingID)
                .ValueGeneratedOnAdd();

            //builder.Entity<Booking>()
            //    .HasOne(b => b.Driver)
            //    .WithMany(b => b.UserID)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
