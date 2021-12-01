using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;
using UniversalParking.BLL.Interfaces;
using UniversalParking.DAL.Entities;
using UniversalParking.DAL.Interfaces;

namespace UniversalParking.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private IWorkUnit database;

        public StatisticsService(IWorkUnit database)
        {
            this.database = database;
        }

        public ParkingStatisticDTO GetFreeParkingPlace(int parkingID)
        {
            var freePlaces = database.ParkingPlaces.GetAll()
                .Where(place => place.ParkingID == parkingID &&
                                place.State == false).Count();

            var parking = database.Parkings.Get(parkingID);
            ParkingStatisticDTO parkingStatisticDTO = new ParkingStatisticDTO()
            {
                ParkingID = parkingID,
                ParkingName = parking.Name,
                AllFree = freePlaces
            };
            return parkingStatisticDTO;                          
        }

        public IEnumerable<ParkingStatisticDTO> GetParkingTop(int userID)
        {
            var bookings = database.Bookings.GetAll();
            var parkingPlaces = database.ParkingPlaces.GetAll();
            var parkings = database.Parkings.GetAll();

            foreach (var place in parkingPlaces)
            {
                place.Parking = parkings
                    .Where(p => p.ParkingID == place.ParkingID)
                    .FirstOrDefault();
            }

            foreach (var booking in bookings)
            {
                booking.ParkingPlace.Parking = parkingPlaces
                    .Where(pl => pl.ParkingPlaceID == booking.ParkingPlaceID)
                    .FirstOrDefault().Parking;
            }

            bookings = bookings
                .Where(b => b.ParkingPlace.Parking.OwnerID == userID)
                .ToList();
            var statistics = from booking in bookings
                             where booking.State == "Left"
                             group booking by (booking.ParkingPlace.Parking.ParkingID,
                                    booking.ParkingPlace.Parking.Name) into bkng
                             orderby bkng.Count() descending
                             select new ParkingStatisticDTO
                             {
                                 ParkingID = bkng.Key.ParkingID,
                                 ParkingName = bkng.Key.Name,
                                 AllBooking = bkng.Count()
                             };

            return statistics;
        }
    }
}
