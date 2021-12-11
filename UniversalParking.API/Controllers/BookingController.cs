using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using UniversalParking.API.Models;
using UniversalParking.BLL.DTO;
using UniversalParking.BLL.Interfaces;
using UniversalParking.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversalParking.API.Controllers
{
    [Authorize]
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingService bookingService;
        private IUserService userService;
        private IParkingPlaceService parkingPlaceService;
        private IMapper mapper;

        public BookingController(IBookingService bookingService,
            IUserService userService, IParkingPlaceService parkingPlaceService)
        {
            this.bookingService = bookingService;
            this.userService = userService;
            this.parkingPlaceService = parkingPlaceService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<BookingModel, BookingDTO>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceModel, ParkingPlaceDTO>().ReverseMap();
                    cfg.CreateMap<ParkingModel, ParkingDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<BookingDTO, BookingModel>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceDTO, ParkingPlaceModel>().ReverseMap();
                    cfg.CreateMap<ParkingDTO, ParkingModel>().ReverseMap();

                })
                .CreateMapper();
        }

        // GET: api/<BookingController>
        [HttpGet]
        public ActionResult<IEnumerable<BookingModel>> Get()
        {
            try
            {
                var bookingsDTO = bookingService.GetAllBookings();
                var bookings = mapper.Map<IEnumerable<BookingDTO>,
                    List<BookingModel>>(bookingsDTO);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<BookingController>/5
        [HttpGet("{id}")]
        public ActionResult<BookingModel> Get(int id)
        {
            try
            {
                var bookingDTO = bookingService.GetBooking(id);
                if (bookingDTO == null)
                {
                    return NotFound();
                }

                var currentBooking = mapper.Map<BookingDTO, BookingModel>(bookingDTO);
                return Ok(currentBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<BookingController>
        [HttpPost]
        public ActionResult<BookingModel> Post([FromBody] BookingModel model)
        {
            try
            {

                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                if (InvalidBookingModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var bookingDTO = mapper.Map<BookingModel, BookingDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(userID));
                bookingDTO.Driver = user;
                var parkingPlace = parkingPlaceService.GetParkingPlace(model.ParkingPlaceID);
                bookingDTO.ParkingPlace = parkingPlace;
                bookingService.AddBooking(bookingDTO);
                return Ok("Booking added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("An booking with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidBookingModel(BookingModel model)
        {
            if (model == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<BookingController>/5
        [HttpPut("{id}")]
        public ActionResult<BookingModel> Put(int id, [FromBody] BookingModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Specify the data you want to change");
                }

                var modelID = HttpContext.User.Identity!.Name;
                if (modelID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var parkingPlace = parkingPlaceService.GetParkingPlace(model.ParkingPlaceID);
                if (parkingPlace == null)
                {
                    return NotFound("There is no parking place with this parkingPlaceID.");
                }

                model.BookingID = id;
                var bookingDTO = mapper.Map<BookingModel, BookingDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(modelID));
                bookingDTO.ParkingPlace = parkingPlace;
                bookingDTO.Driver = user;
                bookingService.UpdateBooking(bookingDTO);
                return Ok("Booking updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<BookingController>/5
        [HttpDelete("{id}")]
        public ActionResult<BookingModel> Delete(int id)
        {
            try
            {
                var bookingModel = bookingService.GetBooking(id);
                if (bookingModel != null)
                {
                    bookingService.DeleteBooking(id);
                    return Ok("Booking deleted successfully.");
                }
                return NotFound("This booking does not exist.");
            }
            catch (Exception)
            {
                return BadRequest("This booking does not exist.");
            }
        }
    }
}
