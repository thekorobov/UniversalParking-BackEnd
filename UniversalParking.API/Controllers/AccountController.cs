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
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMapper mapper;
        UserManager<User> userManager;
        RoleManager<IdentityRole<int>> roleManager;
        private readonly ITokenService tokenService;
        private IUserService userService;

        public AccountController(UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ITokenService tokenService,
            IUserService userService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<UserModel, User>().ReverseMap();
                })
                .CreateMapper();
        }

        [Authorize]
        [Route("profile")]
        [HttpGet]
        public ActionResult<UserModel> Profile()
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var userDTO = this.userService.GetUser(Convert.ToInt32(userID));
                var user = mapper.Map<UserDTO, UserModel>(userDTO);

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<AccountController>
        [Route("signIn")]
        [HttpPost]
        public async Task<ActionResult<UserModel>> SignIn(UserModel userModel)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(userModel.Email);

                if (user == null ||
                    !await userManager.CheckPasswordAsync(user,
                    userModel.Password))
                {
                    return BadRequest("The email or password is incorrect.");
                }
                var userRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                var token = tokenService.GenerateToken(user.Id, userRole);
                var response = new
                {
                    token = token,
                    message = "User logged in successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("signUp")]
        [HttpPost]
        public async Task<ActionResult<UserModel>> SignUp(UserModel userModel)
        {
            try
            {
                if (InvalidUserModel(userModel))
                {
                    return BadRequest("Enter your registration data.");
                }
                if (userModel.Name == null)
                {
                    var index = userModel.Email.IndexOf("@");
                    userModel.Name = userModel.Email.Substring(0, index - 1);
                }
                var user = mapper.Map<UserModel, User>(userModel);
                user.UserName = userModel.Name;
                user.PhoneNumber = userModel.PhoneNumber;

                var managerResult = await userManager.CreateAsync(user,
                    userModel.Password);
                

                var userRoles = from role in roleManager.Roles.ToList()
                                where role.Name == userModel.Role
                                select role.Name;
                var roleResult = await userManager.AddToRolesAsync(user, userRoles);

                if (!managerResult.Succeeded && !roleResult.Succeeded)
                {
                    return BadRequest("A user with such data already exists.");
                }
                return Created("", "User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool InvalidUserModel(UserModel userModel)
        {
            var roleExsist = roleManager.Roles
                .ToList()
                .Exists(role => role.Name == userModel.Role);

            if (userModel == null || userModel.Email == null ||
                userModel.Password == null || !roleExsist)
            {
                return true;
            }
            return false;
        }
    }
}