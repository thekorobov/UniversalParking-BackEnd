using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParking.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Create(T value);
        void Delete(int id);
        void Update(T value);
    }
}
