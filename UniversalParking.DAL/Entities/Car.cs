using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversalParking.DAL.Entities
{
    [Table("car")]
    public class Car
    {
        [Key]
        public int CarID { set; get; }
        public int DriverID;
        public string Model { set; get; }
        public string Brand { set; get; }
        public string CarNumber { set; get; }

        [ForeignKey("DriverID")]
        public virtual User Driver { set; get; }
    }
}
