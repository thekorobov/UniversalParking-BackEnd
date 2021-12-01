using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class ParkingStatisticDTO
    {
        public int ParkingID { set; get; }
        public string ParkingName { set; get; }
        public int AllFree { set; get; }
        public int AllBooking { set; get; }
    }
}
