﻿using System;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="model">User model</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            AuthenticateResponseModel response = await _userService.AuthenticateAsync(new AuthenticateRequestModel
            {
                Secret = _configuration["Security:Secret"],
                Username = model.UserName,
                Password = model.Password,
            });

            if (response == null)
                return Unauthorized(new { message = "Username or password is incorrect" });
       
            return Ok(response);
        }


        /// <summary>
        /// Sign up new user
        /// </summary>
        /// <param name="model">Register user model</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
           await _userService.SignUpAsync(new SignInServiceModel
            {
                Password = model.Password,
                Username = model.UserName
            });

           return Ok();
        }

        /// <summary>
        /// Activate user by activation code
        /// </summary>
        /// <returns></returns>
        [HttpGet("activate/{code}")]
        public async Task<IActionResult> ActivateUser(Guid code)
        {
            await _userService.ActivateUserAsync(code);
          
            return Ok();
        }
    }
}