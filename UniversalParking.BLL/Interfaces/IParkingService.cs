using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface IParkingService
    {
        IEnumerable<ParkingDTO> GetAllParkings();
        ParkingDTO GetParking(int id);
        int AddParking(ParkingDTO parkingDTO);
        void DeleteParking(int id);
        void UpdateParking(ParkingDTO parkingDTO);
    }
}
