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
    [Authorize(Roles = "Administrator,BusinessPartner")]
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsService statisticsService;
        private IParkingService parkingService;
        private IUserService userService;
        private IBookingService bookingService;
        private IMapper mapper;

        public StatisticsController(IStatisticsService statisticsService,
            IParkingService parkingService, IUserService userService)
        {
            this.statisticsService = statisticsService;
            this.parkingService = parkingService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ParkingStatisticDTO,
                        ParkingStatisticModel>().ReverseMap();
                }
                ).CreateMapper();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParkingStatisticModel>> GetParkingTop()
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var parkingTopDTO = this.statisticsService.GetParkingTop(Convert.ToInt32(userID));
                var parkingTop = mapper.Map<IEnumerable<ParkingStatisticDTO>,
                    List<ParkingStatisticModel>>(parkingTopDTO);

                return parkingTop;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{parkingID}")]
        public ActionResult<ParkingStatisticModel> GetFreeParkingPlace(int parkingID)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var currentParking = parkingService.GetParking(parkingID);
                if (currentParking == null)
                {
                    return NotFound("There is no parking with this parkingID.");
                }

                if (currentParking.Owner.UserID != Convert.ToInt32(userID))
                {
                    return BadRequest("You cannot view the statistics of other people's events.");
                }

                var parkingStatisticsDTO =
                    this.statisticsService.GetFreeParkingPlace(parkingID);
                var parkingStatistics = mapper.Map<ParkingStatisticDTO, 
                    ParkingStatisticModel>(parkingStatisticsDTO);

                return parkingStatistics;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("booking/{bookingID}")]
        public ActionResult<BookingStatisticModel> GetFullPriceByBooking(int bookingID)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var currentBooking = bookingService.GetBooking(bookingID);
                if (currentBooking == null)
                {
                    return NotFound("There is no booking with this bookingID.");
                }

                var bookingStatisticsDTO =
                    this.statisticsService.GetFullPriceByBooking(bookingID);
                var bookingStatistics = mapper.Map<BookingStatisticDTO,
                    BookingStatisticModel>(bookingStatisticsDTO);

                return bookingStatistics;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
