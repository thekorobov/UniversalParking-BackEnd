using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.DAL.EF;
using UniversalParking.DAL.Entities;
using UniversalParking.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UniversalParking.DAL.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private UniversalParkingContext database;

        public BookingRepository(UniversalParkingContext database)
        {
            this.database = database;
        }

        public Booking Get(int id)
        {
            return database.bookings
                .Include(booking => booking.Driver)
                .Include(booking => booking.ParkingPlace)
                .SingleOrDefault(booking => booking.BookingID == id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return database.bookings
                .Include(booking => booking.Driver)
                .Include(booking => booking.ParkingPlace)
                .ToList();
        }

        public int Create(Booking booking)
        {
            booking.Driver = database.users
                .Find(booking.Driver.UserID);
            booking.ParkingPlace = database.parkingPlaces
                .Find(booking.ParkingPlace.ParkingPlaceID);
            database.bookings.Add(booking);
            database.SaveChanges();

            return booking.BookingID;
        }

        public void Delete(int id)
        {
            Booking booking = Get(id);
            if(booking != null)
            {
                database.bookings.Remove(booking);
            }
        }

        public void Update(Booking booking)
        {
            var toUpdateBooking = database.bookings.FirstOrDefault(
                b => b.BookingID == b.BookingID);
            if(toUpdateBooking != null)
            {
                toUpdateBooking.BookingID = booking.BookingID;
                toUpdateBooking.Driver = database.users
                    .Find(booking.Driver.UserID);
                toUpdateBooking.ParkingPlace = database.parkingPlaces
                    .Find(booking.ParkingPlace.ParkingPlaceID);
                toUpdateBooking.State = booking.State ?? toUpdateBooking.State;
                toUpdateBooking.StartBooking = booking.StartBooking;
                toUpdateBooking.EndBooking = booking.EndBooking;
            }
           
        }
    }
}
