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
    [Authorize(Roles = "Administrator,Owner")]
    [Route("api/parking")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private IParkingService parkingService;
        private IUserService userService;
        private IMapper mapper;

        public ParkingController(IParkingService parkingService,
            IUserService userService)
        {
            this.parkingService = parkingService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ParkingModel, ParkingDTO>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<ParkingDTO, ParkingModel>().ReverseMap();
                })
                .CreateMapper();
        }

        // GET: api/<ParkingController>
        [HttpGet]
        public ActionResult<IEnumerable<ParkingModel>> Get()
        {
            try
            {
                var parkingsDTO = parkingService.GetAllParkings();
                var parkings = mapper.Map<IEnumerable<ParkingDTO>,
                    List<ParkingModel>>(parkingsDTO);
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<ParkingController>/5
        [HttpGet("{id}")]
        public ActionResult<ParkingModel> Get(int id)
        {
            try
            {
                var parkingDTO = parkingService.GetParking(id);
                if (parkingDTO == null)
                {
                    return NotFound();
                }

                var currentParking = mapper.Map<ParkingDTO, ParkingModel>(parkingDTO);
                return Ok(currentParking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<ParkingController>
        [HttpPost]
        public ActionResult<ParkingModel> Post([FromBody] ParkingModel model)
        {
            try
            {

                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                if (InvalidParkingModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var parkingDTO = mapper.Map<ParkingModel, ParkingDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(userID));
                parkingDTO.Owner = user;
                parkingService.AddParking(parkingDTO);
                return Ok("Parking added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("An event with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidParkingModel(ParkingModel model)
        {
            if (model == null || model.Name == null ||
                model.Address == null || model.NumberSlots == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<ParkingController>/5
        [HttpPut("{id}")]
        public ActionResult<ParkingModel> Put(int id, [FromBody] ParkingModel model)
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

                model.ParkingID = id;
                var parkingDTO = mapper.Map<ParkingModel, ParkingDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(modelID));
                parkingDTO.Owner = user;
                parkingService.UpdateParking(parkingDTO);
                return Ok("Parking updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<ParkingController>/5
        [HttpDelete("{id}")]
        public ActionResult<ParkingModel> Delete(int id)
        {
            try
            {
                var parkingModel = parkingService.GetParking(id);
                if (parkingModel != null)
                {
                    parkingService.DeleteParking(id);
                    return Ok("Parking deleted successfully.");
                }
                return NotFound("This parking does not exist.");
            }
            catch (Exception)
            {
                return BadRequest("This parking does not exist.");
            }
        }
    }
}
