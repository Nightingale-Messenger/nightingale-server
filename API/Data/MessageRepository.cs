using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    class MessageRepository : IMessageRepository
    {
        private readonly DatabaseContext _db;

        public MessageRepository(DatabaseContext db) => _db = db;
        
        public async Task Add(Message item)
        {
            await _db.Messages.AddAsync(item);
        }

        public async Task AddRange(IEnumerable<Message> items)
        {
            await _db.Messages.AddRangeAsync(items);
        }

        public async Task AddRange(params Message[] items)
        {
            await _db.Messages.AddRangeAsync(items);
        }

        public void Remove(Message item)
        {
            _db.Messages.Remove(item);
        }

        public void RemoveRange(IEnumerable<Message> items)
        {
            _db.Messages.RemoveRange(items);
        }

        public void RemoveRange(params Message[] items)
        {
            _db.Messages.RemoveRange(items);
        }

        public async Task RemoveById(int id)
        {
            var b = await _db.Messages.FindAsync(id);
            if (!(b is null))
            {
                _db.Messages.Remove(b);
            }
        }

        public async Task RemoveByIdRange(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var b = await _db.Messages.FindAsync(id);
                if (!(b is null))
                {
                    _db.Messages.Remove(b);
                }
            }
        }

        public async Task RemoveByIdRange(params int[] ids)
        {
            foreach (var id in ids)
            {
                var b = await _db.Messages.FindAsync(id);
                if (!(b is null))
                {
                    _db.Messages.Remove(b);
                }
            }
        }

        public void Update(Message item)
        {
            _db.Update(item);
        }

        public void UpdateRange(IEnumerable<Message> items)
        {
            _db.UpdateRange(items);
        }

        public void UpdateRange(params Message[] items)
        {
            _db.UpdateRange(items);
        }

        public async Task<int> Save()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<Message?> Find(int id)
        {
            return await _db.Messages.FindAsync(id);
        }

        public IEnumerable<Message> GetAll()
        {
            return _db.Messages;
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