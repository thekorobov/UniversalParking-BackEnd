using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.API.Models
{
    public class BookingModel
    {
        public int BookingID { set; get; }
        public int DriverID { set; get; }
        public int ParkingPlaceID { set; get; }
        public string State { set; get; }
        public DateTime StartBooking { set; get; }
        public DateTime EndBooking { set; get; }
        public virtual UserDTO Driver { set; get; }
        public virtual ParkingPlaceDTO ParkingPlace { set; get; }
    }
}
