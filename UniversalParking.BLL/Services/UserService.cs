using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;
using UniversalParking.BLL.Interfaces;
using UniversalParking.DAL.Entities;
using UniversalParking.DAL.Interfaces;

namespace UniversalParking.BLL.Services
{
    public class UserService : IUserService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public UserService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }).CreateMapper();
        }

        public UserDTO GetUser(int id)
        {
            var user = database.Users.Get(id);
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            var userDTO = mapper.Map<User, UserDTO>(user);

            return userDTO;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = database.Users.GetAll()
                .OrderBy(user => user.UserID);
            var usersDTO = mapper.Map<IEnumerable<User>,
                List<UserDTO>>(users);

            return usersDTO;
        }

        public IEnumerable<UserDTO> GetUsersOfOneRole(string role)
        {
            var users = database.Users.GetAll();
            users = users.Where(user => user.Role == role)
                .OrderBy(user => user.UserID)
                .ToList();
            var usersDTO = mapper.Map<IEnumerable<User>,
                List<UserDTO>>(users);

            return usersDTO;
        }

        public void DeleteUser(int id)
        {
            var user = database.Users.Get(id);
            if (user == null)
            {
                throw new NullReferenceException();
            }

            database.Users.Delete(id);
            database.Save();
        }

        public void UpdateUser(UserDTO userDTO)
        {
            var user = database.Users
                .Get(userDTO.UserID);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            var userExsist = database.Users.GetAll()
                .Any(u => u.Email == userDTO.Email && 
                u.UserID == userDTO.UserID);

            if (userExsist)
            {
                throw new NullReferenceException();
            }

            user = mapper.Map<UserDTO, User>(userDTO);
            database.Users.Update(user);
            database.Save();
        }
    }
}
