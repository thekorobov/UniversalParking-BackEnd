using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.DAL.EF;
using UniversalParking.DAL.Entities;
using UniversalParking.DAL.Interfaces;

namespace UniversalParking.DAL.Repositories
{
    public class EFWorkUnit : IWorkUnit
    {
        private UniversalParkingContext database;
        private UserRepository userRepository;
        private ParkingRepository parkingRepository;
        private ParkingPlaceRepository parkingPlaceRepository;
        private CarRepository carRepository;
        private BookingRepository bookingRepository;

        public EFWorkUnit(UniversalParkingContext context)
        {
            database = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(database);
                }
                return userRepository;
            }
        }

        public IRepository<Parking> Parkings
        {
            get
            {
                if (parkingRepository == null)
                {
                    parkingRepository = new ParkingRepository(database);
                }
                return parkingRepository;
            }
        }

        public IRepository<ParkingPlace> ParkingPlaces
        {
            get
            {
                if (parkingPlaceRepository == null)
                {
                    parkingPlaceRepository = new ParkingPlaceRepository(database);
                }
                return parkingPlaceRepository;
            }
        }

        public IRepository<Car> Cars
        {
            get
            {
                if (carRepository == null)
                {
                    carRepository = new CarRepository(database);
                }
                return carRepository;
            }
        }

        public IRepository<Booking> Bookings
        {
            get
            {
                if (bookingRepository == null)
                {
                    bookingRepository = new BookingRepository(database);
                }
                return bookingRepository;
            }
        }

        public void Save()
        {
            database.SaveChanges();
        }
    }
}
