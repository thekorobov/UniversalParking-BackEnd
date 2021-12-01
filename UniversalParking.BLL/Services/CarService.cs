using AutoMapper;
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
    public class CarService : ICarService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public CarService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Car, CarDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<CarDTO, Car>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<CarDTO> GetAllCars()
        {
            var cars = database.Cars.GetAll()
                .OrderBy(c => c.CarID);
            var carsDTO = mapper.Map<IEnumerable<Car>,
                List<CarDTO>>(cars);

            return carsDTO;
        }

        public CarDTO GetCar(int id)
        {
            var currentCar = database.Cars.Get(id);
            if(currentCar == null)
            {
                throw new NullReferenceException("This car does not exist.");
            }
            var carDTO = mapper.Map<Car, CarDTO>(currentCar);

            return carDTO;
        }

        public int AddCar(CarDTO carDTO)
        {
            if(carDTO.Driver == null)
            {
                throw new NullReferenceException();
            }
            var carExsist = database.Cars.GetAll()
                .Any(c => c.Model == carDTO.Model &&
                          c.Driver.UserID == carDTO.Driver.UserID);

            var currentCar = mapper.Map<CarDTO, Car>(carDTO);
            var currentCarID = database.Cars.Create(currentCar);
            return currentCarID;
        }

        public void DeleteCar(int id)
        {
            var currentCar = database.Cars.Get(id);
            if(currentCar == null)
            {
                throw new NullReferenceException("This car does not exist.");
            }
            database.Cars.Delete(id);
            database.Save();
        }

        public void UpdateCar(CarDTO carDTO)
        {
            var currentCar = database.Cars.Get(carDTO.CarID);
            if (currentCar == null)
            {
                throw new NullReferenceException();
            }
            var carExsist = database.Cars.GetAll()
                .Any(c => c.Model == carDTO.Model &&
                          c.Driver.UserID == carDTO.Driver.UserID);
            
            currentCar = mapper.Map<CarDTO, Car>(carDTO);
            database.Cars.Update(currentCar);
            database.Save();
        }
    }
}
