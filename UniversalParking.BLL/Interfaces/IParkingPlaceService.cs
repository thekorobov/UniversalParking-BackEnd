using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface IParkingPlaceService
    {
        IEnumerable<ParkingPlaceDTO> GetAllParkingPlaces();
        public IEnumerable<ParkingPlaceDTO> GetParkingPlaceByParking(int parkingID);
        ParkingPlaceDTO GetParkingPlace(int id);
        int AddParkingPlace(ParkingPlaceDTO parkingPlaceDTO);
        void DeleteParkingPlace(int id);
        void UpdateParkingPlace(ParkingPlaceDTO parkingPlaceDTO);
    }
}
