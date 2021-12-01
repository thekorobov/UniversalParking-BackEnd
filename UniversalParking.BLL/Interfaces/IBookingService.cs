using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<BookingDTO> GetAllBookings();
        BookingDTO GetBooking(int id);
        int AddBooking(BookingDTO bookingDTO);
        void DeleteBooking(int id);
        void UpdateBooking(BookingDTO bookingDTO);
    }
}
