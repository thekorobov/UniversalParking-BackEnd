using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface IStatisticsService
    {
        ParkingStatisticDTO GetFreeParkingPlace(int parkingID);
        IEnumerable<ParkingStatisticDTO> GetParkingTop(int userID);
    }
}
