using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreApp.Data.Repository.Interfaces
{
    public interface IRepository<TType, TId>
    {
        TType GetById(TId id);
        TType GetById(params TId[] id);
        Task<TType> GetByIdAsync(TId id);
        Task<TType> GetByIdAsync(params TId[] id);
        IEnumerable<TType> GetAll();
        Task<IEnumerable<TType>> GetAllAsync();
        IQueryable<TType> GetAllAttached();
        void Add(TType item);
        Task AddAsync(TType item);
        void AddRange(IEnumerable<TType> item);
        Task AddRangeAsync(IEnumerable<TType> item);
        bool Delete(TId id);
        bool Delete(params TId[] id);
        Task<bool> DeleteAsync(TId id);
        Task<bool> DeleteAsync(params TId[] id);
        bool RemoveRange(IEnumerable<TType> items);
        Task<bool> RemoveRangeAsync(IEnumerable<TType> items);
        bool Update(TType item);
        Task<bool> UpdateAsync(TType item);
    }
}
