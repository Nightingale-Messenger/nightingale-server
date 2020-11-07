using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nightingale.Core.Entities;
using Nightingale.Core.Identity;
using Nightingale.Core.Repositories;
using Nightingale.Infrastructure.Data;
using Nightingale.Infrastructure.Repository.Base;

namespace Nightingale.Infrastructure.Repository
{
    public class MessageRepository : Repository<Message, int>, IMessageRepository
    {
        public MessageRepository(NightingaleContext db) : base(db)
        {
        }

        public IEnumerable<Message> GetLastN(int n, string issuerId, string targetId)
        {
            return (from m in _db.Messages
                where m.Sender.Id == issuerId &&
                      m.Receiver.Id == targetId ||
                      m.Sender.Id == targetId &&
                      m.Receiver.Id == issuerId
                orderby m.DateTime descending
                select m).AsNoTracking().Take(n);
        }

        public IEnumerable<User> GetContacts(string userId)
        {
            return (from u in _db.Users
                join ms in _db.Messages on u.Id equals ms.SenderId
                join mr in _db.Messages on u.Id equals mr.ReceiverId
                where ms.SenderId == userId ||
                      mr.ReceiverId == userId
                select u).AsNoTracking();
        }
    }
}