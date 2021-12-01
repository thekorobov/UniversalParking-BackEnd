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
    [Route("api/location")]
    [ApiController]
    public class ParkingPlaceController : ControllerBase
    {
        private IParkingPlaceService parkingPlaceService;
        private IParkingService parkingService;
        private IMapper mapper;

        public ParkingPlaceController(IParkingPlaceService parkingPlaceService,
           IParkingService parkingService)
        {
            this.parkingPlaceService = parkingPlaceService;
            this.parkingService = parkingService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ParkingPlaceModel, ParkingPlaceDTO>().ReverseMap();
                    cfg.CreateMap<ParkingPlaceDTO, ParkingPlaceModel>().ReverseMap();
                    cfg.CreateMap<ParkingModel, ParkingDTO>().ReverseMap();
                    cfg.CreateMap<ParkingDTO, ParkingModel>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                })
                .CreateMapper();
        }

        // GET: api/<ParkingPlaceController>
        [HttpGet]
        public ActionResult<IEnumerable<ParkingPlaceDTO>> Get()
        {
            try
            {
                var parkingPlacesDTO = parkingPlaceService.GetAllParkingPlaces();
                var parkingPlaces = mapper.Map<IEnumerable<ParkingPlaceDTO>,
                    List<ParkingPlaceModel>>(parkingPlacesDTO);
                return Ok(parkingPlaces);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<ParkingPlaceController>/5
        [HttpGet("event/{eventID}")]
        public ActionResult<IEnumerable<ParkingPlaceDTO>> GetParkingPlaceByParking(int parkingID)
        {
            try
            {
                var parkingPlacesDTO = parkingPlaceService.GetParkingPlaceByParking(parkingID);
                var parkingPlaces = mapper.Map<IEnumerable<ParkingPlaceDTO>,
                    List<ParkingPlaceModel>>(parkingPlacesDTO);
                return Ok(parkingPlaces);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<ParkingPlaceController>/5
        [HttpGet("{id}")]
        public ActionResult<ParkingPlaceModel> Get(int id)
        {
            try
            {
                var parkingPlaceDTO = parkingPlaceService.GetParkingPlace(id);
                if (parkingPlaceDTO == null)
                {
                    return NotFound();
                }

                var currentParkingPlace = mapper.Map<ParkingPlaceDTO, ParkingPlaceModel>(parkingPlaceDTO);
                return Ok(currentParkingPlace);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<ParkingPlaceController>
        [HttpPost]
        public ActionResult<ParkingPlaceModel> Post([FromBody] ParkingPlaceModel model)
        {
            try
            {
                var currentParking = parkingService.GetParking(model.ParkingID);
                if (currentParking == null)
                {
                    return NotFound("There is no parking with this parkingID.");
                }
                if (InvalidParkingPlaceModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var parkingPlaceDTO = mapper.Map<ParkingPlaceModel, ParkingPlaceDTO>(model);
                parkingPlaceDTO.Parking = currentParking;
                parkingPlaceService.AddParkingPlace(parkingPlaceDTO);
                return Ok("Parking place added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("A parking place with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidParkingPlaceModel(ParkingPlaceModel model)
        {
            if (model == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<ParkingPlaceController>/5
        [HttpPut("{id}")]
        public ActionResult<ParkingPlaceModel> Put(int id, [FromBody] ParkingPlaceModel model)
        {
            try
            {
                var currentParking = parkingService.GetParking(model.ParkingID);
                if (currentParking == null)
                {
                    return NotFound("There is no parking with this eventID.");
                }

                if (model == null)
                {
                    return BadRequest("Specify the data you want to change");
                }

                var parkingPlaceDTO = mapper.Map<ParkingPlaceModel, ParkingPlaceDTO>(model);
                parkingPlaceDTO.ParkingID = id;
                parkingPlaceDTO.Parking = currentParking;
                parkingPlaceService.UpdateParkingPlace(parkingPlaceDTO);
                return Ok("Parking place updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<ParkingPlaceController>/5
        [HttpDelete("{id}")]
        public ActionResult<ParkingPlaceModel> Delete(int id)
        {
            try
            {
                var parkingPlace = parkingPlaceService.GetParkingPlace(id);
                if (parkingPlace != null)
                {
                    parkingPlaceService.DeleteParkingPlace(id);
                    return Ok("Parking place deleted successfully.");
                }
                return NotFound("This parking place does not exist.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
