using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class BookingStatisticDTO
    {
        public int BookingID { set; get; }
        public string ParkingName { set; get; }
        public string ParkingPlaceName { set; get; }
        public double Price { set; get; }
        public double Penalty { set; get; }
    }
}
