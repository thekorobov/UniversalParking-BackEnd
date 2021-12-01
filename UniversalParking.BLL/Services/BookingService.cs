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
    public class BookingService : IBookingService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public BookingService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Booking, BookingDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<ParkingPlace, ParkingPlaceDTO>().ReverseMap();
                    cfg.CreateMap<BookingDTO, Booking>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceDTO, ParkingPlace>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<BookingDTO> GetAllBookings()
        {
            var bookings = database.Bookings.GetAll()
                .OrderBy(bookings => bookings.BookingID);
            var bookingsDTO = mapper.Map<IEnumerable<Booking>,
                List<BookingDTO>>(bookings);

            return bookingsDTO;
        }

        public BookingDTO GetBooking(int id)
        {
            var booking = database.Bookings.Get(id);
            if(booking == null)
            {
                throw new NullReferenceException();
            }
            var bookingDTO = mapper.Map<Booking, BookingDTO>(booking);

            return bookingDTO;
        }

        public int AddBooking(BookingDTO bookingDTO)
        {
            if(bookingDTO.ParkingPlace == null || bookingDTO.Driver == null)
            {
                throw new ArgumentNullException();
            }
            var bookingExsist = database.Bookings.GetAll()
                .Any(b =>
                     b.ParkingPlace.ParkingPlaceID == bookingDTO.ParkingPlace.ParkingPlaceID &&
                     b.Driver.UserID == bookingDTO.Driver.UserID);
            if (bookingExsist)
            {
                throw new ArgumentException();
            }

            var booking = mapper.Map<BookingDTO, Booking>(bookingDTO);
            var bookingID = database.Bookings.Create(booking);
            return bookingID;
        }

        public void DeleteBooking(int id)
        {
            var booking = database.Bookings.Get(id);
            if(booking == null)
            {
                throw new NullReferenceException();
            }
            database.Bookings.Delete(id);
            database.Save();
        }

        public void UpdateBooking(BookingDTO bookingDTO)
        {
            var booking = database.Bookings.Get(bookingDTO.BookingID);
            if(booking == null)
            {
                throw new NullReferenceException();
            }

            booking = mapper.Map<BookingDTO, Booking>(bookingDTO);
            database.Bookings.Update(booking);
            database.Save();
        }
    }
}
