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
    public class CarRepository : IRepository<Car>
    {
        private UniversalParkingContext database;

        public CarRepository(UniversalParkingContext database)
        {
            this.database = database;
        }

        public Car Get(int id)
        {
            return database.cars
                .Include(car => car.Driver)
                .SingleOrDefault(car => car.CarID == id);
        }

        public IEnumerable<Car> GetAll()
        {
            return database.cars.Include(item => item.Driver).ToList();
        }

        public int Create(Car car)
        {
            car.Driver = database.users
                .Find(car.Driver.Id);
            database.cars.Add(car);
            database.SaveChanges();

            return car.CarID;
        }

        public void Delete(int id)
        {
            Car car = Get(id);
            if (car != null)
            {
                database.cars.Remove(car);
            }
        }

        public void Update(Car car)
        {
            var toUpdateCar = database.cars.FirstOrDefault(
                c => c.CarID == c.CarID);
            if(toUpdateCar != null)
            {
                toUpdateCar.CarID = car.CarID;
                toUpdateCar.Driver = database.users
                    .Find(car.Driver.Id);
                toUpdateCar.Model = car.Model ?? toUpdateCar.Model;
                toUpdateCar.Brand = car.Brand ?? toUpdateCar.Brand;
                toUpdateCar.CarNumber = car.CarNumber ?? toUpdateCar.CarNumber;
            }
        }
    }
}
