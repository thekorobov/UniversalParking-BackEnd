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
    class ParkingRepository : IRepository<Parking>
    {
        private UniversalParkingContext database;

        public ParkingRepository(UniversalParkingContext database)
        {
            this.database = database;
        }

        public Parking Get(int id)
        {
            return database.parkings
                .Include(currentParking => currentParking.Owner)
                .SingleOrDefault(currentParking => currentParking.ParkingID == id);
        }

        public IEnumerable<Parking> GetAll()
        {
            return database.parkings
                .Include(currentParking => currentParking.Owner)
                .ToList();
        }

        public int Create(Parking currentParking)
        {
            currentParking.Owner = database.users
                .Find(currentParking.Owner.UserID);
            database.parkings.Add(currentParking);
            database.SaveChanges();

            return currentParking.ParkingID;
        }

        public void Delete(int id)
        {
            Parking currentParking = Get(id);
            if (currentParking != null)
            {
                database.parkings.Remove(currentParking);
            }
        }

        public void Update(Parking currentParking)
        {
            var toUpdateParking = database.parkings.FirstOrDefault(
                p => p.ParkingID == currentParking.ParkingID);
            if(toUpdateParking != null)
            {
                toUpdateParking.ParkingID = currentParking.ParkingID;
                toUpdateParking.Owner = database.users
                    .Find(currentParking.Owner.UserID);
                toUpdateParking.Name = currentParking.Name ?? toUpdateParking.Name;
                toUpdateParking.Address = currentParking.Address ?? toUpdateParking.Address;
                toUpdateParking.NumberSlots = currentParking.NumberSlots ?? toUpdateParking.NumberSlots;
            }
        }
    }
}
