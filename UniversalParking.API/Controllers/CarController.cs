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
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ICarService carService;
        private IUserService userService;
        private IMapper mapper;

        public CarController(ICarService carService,
            IUserService userService)
        {
            this.carService = carService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CarModel, CarDTO>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<CarDTO, CarModel>().ReverseMap();
                })
                .CreateMapper();
        }

        // GET: api/<CarController>
        [HttpGet]
        public ActionResult<IEnumerable<CarModel>> Get()
        {
            try
            {
                var carsDTO = carService.GetAllCars();
                var cars = mapper.Map<IEnumerable<CarDTO>,
                    List<CarModel>>(carsDTO);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<CarController>/5
        [HttpGet("{id}")]
        public ActionResult<CarModel> Get(int id)
        {
            try
            {
                var carDTO = carService.GetCar(id);
                if (carDTO == null)
                {
                    return NotFound();
                }

                var currentCar = mapper.Map<CarDTO, CarModel>(carDTO);
                return Ok(currentCar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<CarController>
        [HttpPost]
        public ActionResult<CarModel> Post([FromBody] CarModel model)
        {
            try
            {

                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                if (InvalidCarModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var carDTO = mapper.Map<CarModel, CarDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(userID));
                carDTO.Driver = user;
                carService.AddCar(carDTO);
                return Ok("Car added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("An car with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidCarModel(CarModel model)
        {
            if (model == null || model.Model == null ||
                model.Brand == null || model.CarNumber == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<CarController>/5
        [HttpPut("{id}")]
        public ActionResult<CarModel> Put(int id, [FromBody] CarModel model)
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

                model.CarID = id;
                var carDTO = mapper.Map<CarModel, CarDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(modelID));
                carDTO.Driver = user;
                carService.UpdateCar(carDTO);
                return Ok("Car updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<CarController>/5
        [HttpDelete("{id}")]
        public ActionResult<CarModel> Delete(int id)
        {
            try
            {
                var carModel = carService.GetCar(id);
                if (carModel != null)
                {
                    carService.DeleteCar(id);
                    return Ok("Car deleted successfully.");
                }
                return NotFound("This car does not exist.");
            }
            catch (Exception)
            {
                return BadRequest("This car does not exist.");
            }
        }
    }

    
}
