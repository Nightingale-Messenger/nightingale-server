using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nightingale.App.Interfaces;
using Nightingale.App.Mapper;
using Nightingale.App.Models;
using Nightingale.Core.Identity;

namespace Nightingale.App.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;

            _signInManager = signInManager;
        }
        
        public async Task<UserModel> GetByIdAsync(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            return NightingaleMapper.Mapper.Map<UserModel>(user);
        }

        public async Task<OperationDetails> CreateAsync(RegisterModel userModel)
        {
            if (await _userManager.FindByEmailAsync(userModel.Email) != null)
            {
                return new OperationDetails()
                {
                    Succeed = false,
                    Message = "User with such email already exist",
                    Property = "Email"
                };
            }
            
            var user = new User()
            {
                Email = userModel.Email,
                UserName = userModel.UserName
            };
            
            await _userManager.CreateAsync(user, userModel.Password);
            return new OperationDetails()
            {
                Succeed = true,
                Message = "User succesfully created",
                Property = ""
            };
        }

        public async Task<OperationDetails> AuthenticateAsync(LoginModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user == null)
            {
                return new OperationDetails()
                {
                    Succeed = false,
                    Message = "User with such email does not exist",
                    Property = "Email"
                };
            }
            
            var result = await _signInManager.PasswordSignInAsync(user,
                userModel.Password, false, false);
            if (!result.Succeeded)
            {
                return new OperationDetails()
                {
                    Succeed = false,
                    Message = "Invalid email and/or password",
                    Property = "Email/password"
                };
            }
            return new OperationDetails()
            {
                Succeed = true,
                Message = "User succesfully logged in",
                Property = ""
            };
        }

        public async Task<OperationDetails> UpdateAsync(UserModel userModel)
        {
            var result = await _userManager.UpdateAsync(NightingaleMapper.Mapper.Map<User>(userModel));
            if (!result.Succeeded)
            {
                return new OperationDetails()
                {
                    Succeed = false,
                    Message = result.Errors.ToString(),
                    Property = ""
                };
            }
            return new OperationDetails()
            {
                Succeed = true,
                Message = "User succesfully updated",
                Property = ""
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<OperationDetails> ChangePasswordAsync(User user, PasswordChangeModel model)
        {
            var result = await _userManager.ChangePasswordAsync(user,
                model.OldPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                return new OperationDetails()
                {
                    Succeed = false,
                    Message = "Wrong password",
                    Property = ""
                };
            }
            return new OperationDetails()
            {
                Succeed = true,
                Message = "User succesfully updated",
                Property = ""
            };
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
    }
}