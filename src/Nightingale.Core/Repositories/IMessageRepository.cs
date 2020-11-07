using System.Collections.Generic;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;
using Nightingale.Core.Repositories.Base;

namespace Nightingale.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message, int>
    {
        public IEnumerable<Message> GetLastN(int n, string issuerId, string targetId);
    }
}