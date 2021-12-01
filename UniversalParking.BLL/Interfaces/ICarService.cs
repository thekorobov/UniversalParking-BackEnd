using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface ICarService
    {
        IEnumerable<CarDTO> GetAllCars();
        CarDTO GetCar(int id);
        int AddCar(CarDTO carDTO);
        void DeleteCar(int id);
        void UpdateCar(CarDTO carDTO);
    }
}
