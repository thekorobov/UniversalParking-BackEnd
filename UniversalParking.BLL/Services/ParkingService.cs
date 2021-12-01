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
    public class ParkingService : IParkingService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public ParkingService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Parking, ParkingDTO>().ReverseMap();
                    cfg.CreateMap<ParkingPlace, ParkingPlaceDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<ParkingDTO, Parking>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceDTO, ParkingPlace>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<ParkingDTO> GetAllParkings()
        {
            var parkings = database.Parkings.GetAll()
                .OrderBy(p => p.ParkingID);
            var parkingsDTO = mapper.Map<IEnumerable<Parking>,
                List<ParkingDTO>>(parkings);

            return parkingsDTO;
        }

        public ParkingDTO GetParking(int id)
        {
            var currentParking = database.Parkings.Get(id);
            if(currentParking == null)
            {
                throw new NullReferenceException("This parking does not exist.");
            }
            var parkingDTO = mapper.Map<Parking, ParkingDTO>(currentParking);

            return parkingDTO;
        }

        public int AddParking(ParkingDTO parkingDTO)
        {
            if(parkingDTO.Owner == null)
            {
                throw new ArgumentNullException();
            }
            var parkingExsist = database.Parkings.GetAll()
                .Any(p => p.Name == parkingDTO.Name &&
                          p.Owner.UserID == parkingDTO.Owner.UserID);
            if (parkingExsist)
            {
                throw new ArgumentException("An parking with this name already exists.");
            }

            var currentParking = mapper.Map<ParkingDTO, Parking>(parkingDTO);
            var currentParkingID = database.Parkings.Create(currentParking);
            return currentParkingID;
        }

        public void DeleteParking(int id)
        {
            var currentParking = database.Parkings.Get(id);
            if (currentParking == null)
            {
                throw new NullReferenceException("This parking does not exist.");
            }

            database.Parkings.Delete(id);
            database.Save();
        }

        public void UpdateParking(ParkingDTO parkingDTO)
        {
            var currentParking = database.Parkings.Get(parkingDTO.ParkingID);
            if(currentParking == null)
            {
                throw new NullReferenceException();
            }
            var parkingExsist = database.Parkings.GetAll()
                .Any(p => p.Name == parkingDTO.Name &&
                          p.Owner.UserID == parkingDTO.Owner.UserID);
            if (parkingExsist)
            {
                throw new NullReferenceException("An parking with this name already exists");
            }

            currentParking = mapper.Map<ParkingDTO, Parking>(parkingDTO);
            database.Parkings.Update(currentParking);
            database.Save();
        }
    }
}
