using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.DAL.Entities;

namespace UniversalParking.DAL.Interfaces
{
    public interface IWorkUnit
    {
        IRepository<User> Users { get; }
        IRepository<Car> Cars { get; }
        IRepository<Parking> Parkings { get; }
        IRepository<ParkingPlace> ParkingPlaces { get; }
        IRepository<Booking> Bookings { get; }
        void Save();
    }
}
