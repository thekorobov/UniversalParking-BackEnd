using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.API.Models
{
    public class CarModel
    {
        public int CarID { set; get; }
        public string Model { set; get; }
        public string Brand { set; get; }
        public string CarNumber { set; get; }
        public UserDTO Driver { set; get; }
    }
}
