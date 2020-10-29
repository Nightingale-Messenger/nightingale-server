using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    class MessageRepository : IRepository<Message, int>
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
            _db.Entry(item).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<Message> items)
        {
            foreach (var item in items)
            {
                _db.Entry(item).State = EntityState.Modified;
            }
        }

        public void UpdateRange(params Message[] items)
        {
            foreach (var item in items)
            {
                _db.Entry(item).State = EntityState.Modified;
            }
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
    }
}