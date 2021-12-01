using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class CarDTO
    {
        public int CarID { set; get; }
        public string Model { set; get; }
        public string Brand { set; get; }
        public string CarNumber { set; get; }
        public UserDTO Driver { set; get; }
    }
}
