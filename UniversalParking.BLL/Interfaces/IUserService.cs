using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        IEnumerable<UserDTO> GetUsersOfOneRole(string role);
        UserDTO GetUser(int id);
        void DeleteUser(int id);
        void UpdateUser(UserDTO visitorDTO);
    }
}
