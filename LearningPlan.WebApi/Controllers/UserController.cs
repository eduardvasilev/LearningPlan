using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            var response = await _userService.AuthenticateAsync(new AuthenticateRequestModel
            {
                Secret = "secretsecretsecret",
                Username = model.UserName,
                Password = model.Password
            });

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
           await _userService.SignInAsync(new SignInServiceModel
            {
                Password = model.Password,
                Username = model.UserName
            });

           return Ok();
        }
    }
}