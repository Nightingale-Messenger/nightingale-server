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
                Email = registerModel.Email,
                UserName = registerModel.Email.Split('@')[0]
            };
            Console.WriteLine(user.UserName);
            try
            {
                var r = await _userManager.CreateAsync(user, registerModel.Password);
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
                var s = await _signInManager.PasswordSignInAsync(
                    user, registerModel.Password, true, false);
                
                if (s.IsLockedOut) return BadRequest(new {message = "User is locked out"});
                if (s.IsNotAllowed) return BadRequest(new {message = "User is not allowed"});
                if (s.RequiresTwoFactor) return BadRequest(new {message = "User requires 2FA"});

                var jwt = TokenCreator.CreateToken(_jwtConfiguration, user);
                return Ok(new Token
                {
                    AccessToken = jwt,
                    User = user
                });
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

                var s = await _signInManager.PasswordSignInAsync(
                    user, loginModel.Password, false, false);
                
                if (s.IsLockedOut) return BadRequest(new {message = "User is locked out"});
                if (s.IsNotAllowed) return BadRequest(new {message = "User is not allowed"});
                if (s.RequiresTwoFactor) return BadRequest(new {message = "User requires 2FA"});
                
                var jwt = TokenCreator.CreateToken(_jwtConfiguration, user);
                return Ok(new Token()
                {
                    AccessToken = jwt,
                    User = user
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