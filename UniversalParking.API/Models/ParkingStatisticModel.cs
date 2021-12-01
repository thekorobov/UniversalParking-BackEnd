using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.API.Models
{
    public class ParkingStatisticModel
    {
        public int ParkingID { set; get; }
        public string ParkingName { set; get; }
        public int AllFree { set; get; }
        public int AllBooking { set; get; }
    }
}
