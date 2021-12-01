using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.API.Models
{
    public class ParkingModel
    {
        public int ParkingID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string NumberSlots { set; get; }
        public UserDTO Owner { set; get; }
    }
}
