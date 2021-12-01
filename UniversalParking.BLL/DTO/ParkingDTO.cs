using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class ParkingDTO
    {
        public int ParkingID { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string NumberSlots { set; get; }
        public UserDTO Owner { set; get; }
    }
}
