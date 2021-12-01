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
    class ParkingPlaceRepository : IRepository<ParkingPlace>
    {
        private UniversalParkingContext database;

        public ParkingPlaceRepository(UniversalParkingContext database)
        {
            this.database = database;
        }

        public ParkingPlace Get(int id)
        {
            return database.parkingPlaces
                .Include(place => place.Parking)
                .SingleOrDefault(place => place.ParkingPlaceID == id);
        }

        public IEnumerable<ParkingPlace> GetAll()
        {
            return database.parkingPlaces
                .Include(place => place.Parking)
                .ToList();
        }

        public int Create(ParkingPlace place)
        {
            place.Parking = database.parkings
                .Find(place.Parking.ParkingID);
            database.parkingPlaces.Add(place);
            database.SaveChanges();

            return place.ParkingPlaceID;
        }

        public void Delete(int id)
        {
            ParkingPlace place = Get(id);
            if(place != null)
            {
                database.parkingPlaces.Remove(place);
            }
        }

        public void Update(ParkingPlace place)
        {
            var toUpdatePlace = database.parkingPlaces.FirstOrDefault(
                parkingPlaces => parkingPlaces.ParkingPlaceID == place.ParkingPlaceID);
            if(toUpdatePlace != null)
            {
                toUpdatePlace.ParkingPlaceID = place.ParkingPlaceID;
                toUpdatePlace.Parking = database.parkings
                    .Find(place.Parking.ParkingID);
                toUpdatePlace.Name = place.Name ?? place.Name;
                toUpdatePlace.Price = place.Price;
                toUpdatePlace.State = place.State;
            }
        }
    }
}
