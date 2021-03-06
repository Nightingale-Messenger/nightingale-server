using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nightingale.App.Interfaces;
using Nightingale.App.Models;
using Nightingale.App.Services;

namespace Nightingale.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        [Route("change/password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel model)
        {
            var user = await _userService.GetUserAsync(User);

            var result = await _userService.ChangePasswordAsync(user, model);

            if (result.Succeed)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Route("info/{id}")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return BadRequest(new OperationDetails()
                {
                    Message = "User with specified id does not exist",
                    Property = "id",
                    Succeed = false
                });
            return Ok(user);
        }

    }
}