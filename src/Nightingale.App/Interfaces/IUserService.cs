using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Nightingale.App.Models;
using Nightingale.Core.Identity;

namespace Nightingale.App.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(string id);

        Task<User> GetUserAsync(ClaimsPrincipal user);

        Task<User> FindByEmailAsync(string email);

        Task<OperationDetails> CreateAsync(RegisterModel userModel);

        Task<OperationDetails> AuthenticateAsync(LoginModel userModel);

        Task<OperationDetails> UpdateAsync(UserModel userModel);

        Task Logout();

        Task<OperationDetails> ChangePasswordAsync(User user, PasswordChangeModel passwordChangeModel);

        Task<IEnumerable<UserModel>> FindByPublicUserNameAsync(string publicUserName);
    }
}
