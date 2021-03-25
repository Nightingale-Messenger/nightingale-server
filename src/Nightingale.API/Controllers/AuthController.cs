using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nightingale.API.Models;
using Nightingale.API.Services;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;
using Nightingale.Infrastructure.Data;

namespace Nightingale.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly NightingaleContext _appContext;
        public AuthController(IUserService userService,
            IJwtService jwtService,
            NightingaleContext appContext)
        {
            _userService = userService;
            _jwtService = jwtService;
            _appContext = appContext;
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> CheckStatus()
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated) return Ok(new OperationDetails()
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
            var user = await _userService.FindByEmailAsync(loginModel.Email);
            var tokens = _jwtService.GenerateTokens(loginModel.Email, new []
            {
                new Claim("Id", user.Id),
                new Claim("Username", user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Email", loginModel.Email)
            });
            tokens.RefreshToken.User = await _userService.FindByEmailAsync(loginModel.Email);
            await _appContext.RefreshTokens.AddAsync(tokens.RefreshToken);
            await _appContext.SaveChangesAsync();
            return Ok(new {AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken.Token});
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var result = await _userService.CreateAsync(registerModel);
            if (!result.Succeed) return BadRequest(result);
            // await _userService.AuthenticateAsync(new LoginModel()
            // {
            //     Email = registerModel.Email,
            //     Password = registerModel.Password
            // });
            
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

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel refreshModel)
        {
            // if (!Request.Headers.ContainsKey("Authorization"))
            // {
            //     return Unauthorized("Bearer token required");
            // }
            
            var dbToken = _appContext.RefreshTokens
                .Include(t => t.User)
                .SingleOrDefault(t => t.Token.Equals(refreshModel.RefreshToken));
            if (dbToken == null)
            {
                return Unauthorized(new OperationDetails()
                {
                    Message = "Provided refresh token is invalid",
                    Succeed = false,
                    Property = "token"
                });
            }

            if (dbToken.ExpirationDate < DateTime.Now)
            {
                _appContext.RefreshTokens.Remove(dbToken);
                await _appContext.SaveChangesAsync();
                return Unauthorized(new OperationDetails()
                {
                    Message = "Provided refresh token is expired",
                    Succeed = false,
                    Property = "token"
                });
            }
            
            var newTokens = _jwtService.GenerateTokens(dbToken.Email, new []
            {
                new Claim("Id", dbToken.User.Id),
                new Claim("Username", dbToken.User.UserName),
                new Claim(ClaimTypes.NameIdentifier, dbToken.User.Id),
                new Claim("Email", dbToken.User.Email)
            });
            
            dbToken.Token = newTokens.RefreshToken.Token;
            dbToken.ExpirationDate = newTokens.RefreshToken.ExpirationDate;
            await _appContext.SaveChangesAsync();
            return Ok(new {AccessToken = newTokens.AccessToken, RefreshToken = newTokens.RefreshToken.Token});
        }

        [HttpGet]
        [Authorize]
        [Route("logins")]
        public async Task<IActionResult> GetLogins()
        {
            var user = await _userService.GetUserAsync(User);
            return Ok(from token in user.RefreshTokens select token.Token);
        }

        [HttpPost]
        [Authorize]
        [Route("revoke")]
        public async Task<IActionResult> RevokeAccessToken([FromBody] RefreshModel refreshModel)
        {
            var user = await _userService.GetUserAsync(User);
            var token = _appContext.RefreshTokens
                .SingleOrDefault(t => t.Token == refreshModel.RefreshToken &&
                                      t.Email == user.Email);
            if (token == null)
            {
                return BadRequest(new OperationDetails()
                {
                    Message = "Specified token does not exist",
                    Succeed = false,
                    Property = "token"
                });
            }

            var res = _appContext.RefreshTokens.Remove(token);

            await _appContext.SaveChangesAsync();
            return Ok(new OperationDetails()
            {
                Message = "Refresh Token successfully deleted",
                Succeed = true
            });
        }
    }
}