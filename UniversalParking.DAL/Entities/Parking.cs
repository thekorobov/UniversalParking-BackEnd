using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalParking.DAL.Entities
{
    [Table("parking")]
    public class Parking
    {
        [Key]
        public int ParkingID { set; get; }
        public int OwnerID;
        public string Name { set; get; }
        public string Address { set; get; }
        public string NumberSlots { set; get; }

        [ForeignKey("OwnerID")]
        public virtual User Owner { set; get; }
    }
}
