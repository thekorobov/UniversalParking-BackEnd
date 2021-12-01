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

namespace UniversalParking.API.Controllers
{
    [Route("api/administrator")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private IUserService service;
        private IMapper mapper;

        public AdministratorController(IUserService service)
        {
            this.service = service;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<UserModel, User>().ReverseMap();
                })
                .CreateMapper();

        }

        // GET: api/<AdministratorController>
        [Authorize(Roles = "Administrator")]
        [HttpGet("{role}")]
        public ActionResult<IEnumerable<UserModel>> Get(string role)
        {
            try
            {
                switch (role)
                {
                    case "driver":
                        role = "Driver";
                        break;
                    case "owner":
                        role = "Owner";
                        break;
                    default:
                        role = "Administrator";
                        break;
                }

                var usersDTO = service.GetUsersOfOneRole(role);
                var users = mapper.Map<IEnumerable<UserDTO>,
                    List<UserModel>>(usersDTO);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/<AdministratorController>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            try
            {
                var usersDTO = service.GetAllUsers();
                var users = mapper.Map<IEnumerable<UserDTO>,
                    List<UserModel>>(usersDTO);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id:int}")]
        public ActionResult<UserModel> Get(int id)
        {
            try
            {
                var userDTO = service.GetUser(id);
                if (userDTO == null)
                {
                    return NotFound();
                }
                var user = mapper.Map<UserDTO, UserModel>(userDTO);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UserModel model)
        {
            try
            {
                var user = service.GetUser(id);
                if (user != null)
                {
                    var userDTO = mapper.Map<UserModel, UserDTO>(model);
                    userDTO.UserID = id;
                    service.UpdateUser(userDTO);
                    return Ok("Changes made successfully.");
                }
                return NotFound("This user does not exist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = service.GetUser(id);
                if (user != null)
                {
                    service.DeleteUser(id);
                    return Ok("User deleted successfully.");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
