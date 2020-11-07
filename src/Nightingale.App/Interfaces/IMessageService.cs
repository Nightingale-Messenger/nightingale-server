using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.App.Models;
using Nightingale.Core.Identity;

namespace Nightingale.App.Interfaces
{
    public interface IMessageService
    {
        Task<MessageModel> Create(UserModel sender, UserModel receiver, string text, DateTime dateTime);
        Task<IEnumerable<MessageModel>> GetLastN(int n, string issuerId, string targetId);

        Task<IEnumerable<UserModel>> GetContacts(string userId);

        Task<IEnumerable<MessageModel>> GetMessagesBeforeId(int id);
    }
}