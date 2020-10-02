using System;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using API.Utils;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _jwtConfiguration;
        
        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtConfiguration = configuration.GetSection("TokenOptions");
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckStatus()
        {
            _logger.LogInformation("CheckStatus called");  
            if (await _userManager.GetUserAsync(User) == null)
            {
                return Unauthorized();
            }
            return Ok();
        }
        
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            _logger.LogInformation("Register has been called");
            var user = new User()
            {
                Email = registerModel.Email
            };
            user.UserName = user.Id;
            try
            {
                await _userManager.CreateAsync(user, registerModel.Password);
                // await _signInManager.PasswordSignInAsync(
                //     user, registerModel.Password, true, false);
                return await Login(new LoginModel()
                {
                    Email = registerModel.Email,
                    Password = registerModel.Password
                });
                //return Created("", user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            _logger.LogInformation("Login has been called");
            try
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                await _signInManager.PasswordSignInAsync(
                    user, loginModel.Password, false, false);
                var jwt = TokenCreator.CreateToken(_jwtConfiguration, user);
                return Ok(new Token()
                {
                    AccessToken = jwt,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Route("Logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Logout has been called");
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}