using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nightingale.App.Models;
using Nightingale.App.Services;

namespace Nightingale.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
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
    }
}