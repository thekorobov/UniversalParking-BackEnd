using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.DAL.Entities
{
    [Table("user")]
    public class User : IdentityUser<int>
    {
        [Key]
        public int UserID { set; get; }
        public string Name { set; get; }
        public string Role { set; get; }
    }
}
