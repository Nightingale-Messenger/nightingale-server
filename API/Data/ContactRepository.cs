using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    class ContactRepository:IRepository<Contact, int>
    {
        private readonly DatabaseContext _db;
        
        public ContactRepository(DatabaseContext db) => _db = db;
        
        public async Task Add(Contact item)
        {
            await _db.Contacts.AddAsync(item);
        }

        public async Task AddRange(IEnumerable<Contact> items)
        {
            await _db.Contacts.AddRangeAsync(items);
        }

        public async Task AddRange(params Contact[] items)
        {
            await _db.Contacts.AddRangeAsync(items);
        }

        public void Remove(Contact item)
        {
            _db.Contacts.Remove(item);
        }

        public void RemoveRange(IEnumerable<Contact> items)
        {
            _db.Contacts.RemoveRange(items);
        }

        public void RemoveRange(params Contact[] items)
        {
            _db.Contacts.RemoveRange(items);
        }

        public async Task RemoveById(int id)
        {
            var b = await _db.Contacts.FindAsync(id);
            if (!(b is null))
            {
                _db.Contacts.Remove(b);
            }
        }

        public async Task RemoveByIdRange(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var b = await _db.Contacts.FindAsync(id);
                if (!(b is null))
                {
                    _db.Contacts.Remove(b);
                }
            }
        }
        
        public async Task RemoveByIdRange(params int[] ids)
        {
            foreach (var id in ids)
            {
                var b = await _db.Contacts.FindAsync(id);
                if (!(b is null))
                {
                    _db.Contacts.Remove(b);
                }
            }
        }

        public void Update(Contact item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<Contact> items)
        {
            foreach (var item in items)
            {
                _db.Entry(item).State = EntityState.Modified;
            }
        }

        public void UpdateRange(params Contact[] items)
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

        public async Task<Contact?> Find(int id)
        {
            return await _db.Contacts.FindAsync(id);
        }

        public IEnumerable<Contact> GetAll()
        {
            return _db.Contacts;
        }
    }
}