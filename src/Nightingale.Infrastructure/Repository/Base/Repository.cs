using System.Collections.Generic;
using System.Threading.Tasks;
using Nightingale.Core.Entities.Base;
using Nightingale.Core.Repositories.Base;
using Nightingale.Infrastructure.Data;

namespace Nightingale.Infrastructure.Repository.Base
{
    public class Repository<T, TId> : IRepository<T, TId> where T : EntityBase<TId>
    {
        protected readonly NightingaleContext _db;

        public Repository(NightingaleContext db) => _db = db;


        public async Task Add(T item)
        {
            await _db.Set<T>().AddAsync(item);
        }

        public async Task AddRange(IEnumerable<T> items)
        {
            await _db.Set<T>().AddRangeAsync(items);
        }

        public async Task AddRange(params T[] items)
        {
            await _db.Set<T>().AddRangeAsync(items);
        }

        public void Remove(T item)
        {
            _db.Set<T>().Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            _db.Set<T>().RemoveRange(items);
        }

        public void RemoveRange(params T[] items)
        {
            _db.Set<T>().RemoveRange(items);
        }

        public async Task RemoveById(TId id)
        {
            var b = await _db.Messages.FindAsync(id);
            if (!(b is null))
            {
                _db.Messages.Remove(b);
            }
        }

        public async Task RemoveByIdRange(IEnumerable<TId> ids)
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

        public async Task RemoveByIdRange(params TId[] ids)
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

        public void Update(T item)
        {
            _db.Update(item);
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            _db.UpdateRange(items);
        }

        public void UpdateRange(params T[] items)
        {
            _db.UpdateRange(items);
        }

        public async Task<int> Save()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<T> Find(TId id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>();
        }
    }
}