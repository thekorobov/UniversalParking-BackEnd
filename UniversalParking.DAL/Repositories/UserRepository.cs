using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.DAL.EF;
using UniversalParking.DAL.Entities;
using UniversalParking.DAL.Interfaces;

namespace UniversalParking.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private UniversalParkingContext database;

        public UserRepository(UniversalParkingContext database)
        {
            this.database = database;
        }

        public User Get(int id)
        {
            return database.users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return database.users.ToList();
        }

        public int Create(User user)
        {
            database.users.Add(user);
            database.SaveChanges();

            return user.Id;
        }

        public void Delete(int id)
        {
            User user = Get(id);
            if (user != null)
            {
                database.users.Remove(user);
            }
        }

        public void Update(User user)
        {
            var toUpdateUser = database.users.FirstOrDefault(
                user => user.Id == user.Id);
            if (toUpdateUser != null)
            {
                toUpdateUser.Id = user.Id;
                toUpdateUser.Name = user.Name ?? toUpdateUser.Name;
                toUpdateUser.Email = user.Email ?? toUpdateUser.Email;
                toUpdateUser.PasswordHash = user.PasswordHash ?? toUpdateUser.PasswordHash;
                toUpdateUser.PhoneNumber = user.PhoneNumber ?? toUpdateUser.PhoneNumber;
                toUpdateUser.Role = user.Role ?? toUpdateUser.Role;
            }
        }
    }
}
