using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class ParkingPlaceDTO
    {
        public int ParkingPlaceID { set; get; }
        public int ParkingID { set; get; }
        public string Name { set; get; }
        public double Price { set; get; }
        public bool State { set; get; }

        public ParkingDTO Parking { set; get; }
    }
}
