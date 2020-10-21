using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly DatabaseContext _dbContext; 

        public UserController(
            UserManager<User> userManager,
            DatabaseContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Route("username")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUsername(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                user.UserName = username;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("publicusername")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePublicUserName([FromBody] string NewPublicUserName)
        {
            var user = await _userManager.GetUserAsync(User);

            if (_dbContext.Users.SingleOrDefault(u => u.PublicUserName == NewPublicUserName) != null)
            {
                return BadRequest(new { message = "PublicUserName in use"});
            }

            user.PublicUserName = NewPublicUserName;
            await _userManager.UpdateAsync(user);

            return Ok(user.PublicUserName);
        }

        [Route("password")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody]PasswordChangeModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                await _userManager.ChangePasswordAsync(user, model.oldPwd, model.newPwd);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}