using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data
{
    internal interface IRepository<T, in TId> where T: class
    {
        Task Add(T item);
        Task AddRange(IEnumerable<T> items);
        Task AddRange(params T[] items);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
        void RemoveRange(params T[] items);
        Task RemoveById(TId id);
        Task RemoveByIdRange(IEnumerable<TId> ids);
        Task RemoveByIdRange(params TId[] ids);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void UpdateRange(params T[] items);
        Task<int> Save();
        Task<T?> Find(TId id);
        IEnumerable<T> GetAll();
    }
}