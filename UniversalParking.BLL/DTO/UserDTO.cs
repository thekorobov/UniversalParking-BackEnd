using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.BLL.DTO
{
    public class UserDTO
    {
        public int UserID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Password { set; get; }
        public string Role { set; get; }
    }
}
