using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.App.Models;
using Nightingale.Core.Identity;

namespace Nightingale.App.Interfaces
{
    public interface IHubService
    {
        Task<MessageModel> SendAsync(NewMessageModel msg);

        Task<IEnumerable<MessageModel>> GetLastMessagesAsync(string issuerId, string receiverId);

        Task<IEnumerable<UserModel>> GetContacts(User user);

        Task<IEnumerable<UserModel>> FindByPublicUserName(string publicUserName);

        Task<IEnumerable<MessageModel>> GetMessagesBeforeId(int id);

        Task<bool> CheckMessagePermission(int messageId, string userId);
    }
}