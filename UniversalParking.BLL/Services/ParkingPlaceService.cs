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
    public class ParkingPlaceService : IParkingPlaceService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public ParkingPlaceService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ParkingPlace, ParkingPlaceDTO>().ReverseMap();
                    cfg.CreateMap<Parking, ParkingDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceDTO, ParkingPlace>().ReverseMap();
                    cfg.CreateMap<ParkingDTO, Parking>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<ParkingPlaceDTO> GetAllParkingPlaces()
        {
            var places = database.ParkingPlaces.GetAll()
                .OrderBy(place => place.ParkingPlaceID);
            var placesDTO = mapper.Map<IEnumerable<ParkingPlace>,
                List<ParkingPlaceDTO>>(places);

            return placesDTO;
        }

        public IEnumerable<ParkingPlaceDTO> GetParkingPlaceByParking(int parkingID)
        {
            var parkingPlace = database.ParkingPlaces.GetAll();
            parkingPlace = parkingPlace.Where(pl =>
                pl.Parking.ParkingID == parkingID)
                .OrderBy(pl => pl.ParkingPlaceID)
                .ToList();
            var parkingPlaceDTO = mapper.Map<IEnumerable<ParkingPlace>,
                List<ParkingPlaceDTO>>(parkingPlace);

            return parkingPlaceDTO;
        }
        public ParkingPlaceDTO GetParkingPlace(int id)
        {
            var place = database.ParkingPlaces.Get(id);
            if(place == null)
            {
                throw new NullReferenceException();
            }
            var placeDTO = mapper.Map<ParkingPlace, ParkingPlaceDTO>(place);

            return placeDTO;
        }

        public int AddParkingPlace(ParkingPlaceDTO parkingPlaceDTO)
        {
            if(parkingPlaceDTO.Parking == null)
            {
                throw new ArgumentNullException();
            }
            var placeExsist = database.ParkingPlaces.GetAll()
                .Any(pl => pl.Name == parkingPlaceDTO.Name &&
                           pl.Parking.ParkingID == parkingPlaceDTO.Parking.ParkingID);
            if (placeExsist)
            {
                throw new ArgumentException("This place already exists at this event.");
            }

            var parkingPlace = mapper.Map<ParkingPlaceDTO, ParkingPlace>(parkingPlaceDTO);
            var parkingPlaceID = database.ParkingPlaces.Create(parkingPlace);
            return parkingPlaceID;
        }

        public void DeleteParkingPlace(int id)
        {
            var parkingPlace = database.ParkingPlaces.Get(id);
            if(parkingPlace == null)
            {
                throw new NullReferenceException();
            }
            database.ParkingPlaces.Delete(id);
            database.Save();
        }

        public void UpdateParkingPlace(ParkingPlaceDTO parkingPlaceDTO)
        {
            var parkingPlace = database.ParkingPlaces.Get(parkingPlaceDTO.ParkingPlaceID);
            if(parkingPlace == null)
            {
                throw new NullReferenceException();
            }
            var parkingPlaceExsist = database.ParkingPlaces.GetAll()
                .Any(pl => pl.ParkingPlaceID != parkingPlaceDTO.ParkingPlaceID && 
                           pl.Name == parkingPlaceDTO.Name &&
                           pl.Parking.ParkingID == parkingPlaceDTO.Parking.ParkingID);
            if (parkingPlaceExsist)
            {
                throw new NullReferenceException();
            }

            parkingPlace = mapper.Map<ParkingPlaceDTO, ParkingPlace>(parkingPlaceDTO);
            database.ParkingPlaces.Update(parkingPlace);
            database.Save();
        }
    }
}
