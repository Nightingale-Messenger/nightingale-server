using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;

namespace Nightingale.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> CheckStatus()
        {
            if (User.Identity.IsAuthenticated) return Ok(new OperationDetails()
            {
                Succeed = true,
                Message = "User logged in",
                Property = ""
            });
            return Unauthorized(new OperationDetails()
            {
                Succeed = false,
                Message = "User not logged in",
                Property = ""
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var result = await _userService.AuthenticateAsync(loginModel);
            if (!result.Succeed) return BadRequest(result);
            
            return Ok(result);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var result = await _userService.CreateAsync(registerModel);
            if (!result.Succeed) return BadRequest(result);
            await _userService.AuthenticateAsync(new LoginModel()
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            });
            
            return Ok(result);
        }
        
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Ok();
        }
    }
}