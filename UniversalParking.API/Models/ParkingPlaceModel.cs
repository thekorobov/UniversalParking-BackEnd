using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.API.Models
{
    public class ParkingPlaceModel
    {
        public int ParkingPlaceID { set; get; }
        public int ParkingID { set; get; }
        public string Name { set; get; }
        public decimal Price { set; get; }
        public bool State { set; get; }

        public ParkingDTO Parking { set; get; }
    }
}
