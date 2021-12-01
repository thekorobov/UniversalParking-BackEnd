using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalParking.DAL.Entities
{
    [Table("booking")]
    public class Booking
    {
        [Key]
        public int BookingID { set; get; }
        public int DriverID;
        public int ParkingPlaceID;
        public string State { set; get; }
        public DateTime StartBooking { set; get; }
        public DateTime EndBooking { set; get; }

        [ForeignKey("DriverID")]
        public virtual User Driver { set; get; }

        [ForeignKey("ParkingPlaceID")]
        public virtual ParkingPlace ParkingPlace { set; get; }
    }
}
