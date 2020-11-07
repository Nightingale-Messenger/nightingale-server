using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;
using Nightingale.Core.Repositories.Base;

namespace Nightingale.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message, int>
    {
        Task<IEnumerable<Message>> GetLastN(int n, string issuerId, string targetId);

        Task<IEnumerable<User>> GetContacts(string userId);

        Task<IEnumerable<Message>> GetMessagesBeforeId(int n, int id);
    }
}