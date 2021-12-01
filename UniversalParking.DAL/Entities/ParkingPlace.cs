using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalParking.DAL.Entities
{
    [Table("parkingPlace")]
    public class ParkingPlace
    {
        [Key]
        public int ParkingPlaceID { set; get; }
        public int ParkingID;
        public string Name { set; get; }
        public decimal Price { set; get; }
        public bool State { set; get; }

        [ForeignKey("ParkingID")]
        public virtual Parking Parking { set; get; }
    }
}