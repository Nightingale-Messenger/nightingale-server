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

        public IEnumerable<Message> GetLastN(int n, User issuer, User target)
        {
            return (from m in _db.Messages
                where m.Sender.Id == issuer.Id &&
                      m.Receiver.Id == target.Id ||
                      m.Sender.Id == target.Id &&
                      m.Receiver.Id == issuer.Id
                orderby m.DateTime descending
                select m).AsNoTracking().Take(n);
        }
    }
}