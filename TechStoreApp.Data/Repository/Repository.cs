using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreApp.Data.Repository.Interfaces;

namespace TechStoreApp.Data.Repository
{
    public class Repository<TType, TId> : IRepository<TType, TId>
        where TType : class
    {
        private readonly TechStoreDbContext context;
        private readonly DbSet<TType> dbSet;
        public Repository(TechStoreDbContext _context)
        {
            context = _context;
            dbSet = context.Set<TType>();
        }

        public IEnumerable<TType> GetAll()
        {
            return dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return dbSet.AsQueryable();
        }

        public TType GetById(TId id)
        {
            return dbSet.Find(id);
        }
        public TType GetById(params TId[] id)
        {
            return dbSet.Find(id[0], id[1]);
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<TType> GetByIdAsync(params TId[] id)
        {
            return await dbSet.FindAsync(id[0], id[1]);
        }

        public void Add(TType item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await dbSet.AddAsync(item);
            await context.SaveChangesAsync();
        }
        public void AddRange(IEnumerable<TType> item)
        {
            dbSet.AddRange(item);
            context.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TType> item)
        {
            await dbSet.AddRangeAsync(item);
            await context.SaveChangesAsync();
        }

        public bool Delete(TId id)
        {
            TType entity = GetById(id);

            if (entity == null)
            {
                return false;
            }

            dbSet.Remove(entity);
            context.SaveChanges();

            return true;
        }
        public bool Delete(params TId[] id)
        {
            TType entity = GetById(id[0], id[1]);

            if (entity == null)
            {
                return false;
            }

            dbSet.Remove(entity);
            context.SaveChanges();

            return true;
        }
        public async Task<bool> DeleteAsync(TId id)
        {
            TType entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            dbSet.Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(params TId[] id)
        {
            TType entity = await GetByIdAsync(id[0], id[1]);

            if (entity == null)
            {
                return false;
            }

            dbSet.Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }
        public bool Update(TType item)
        {
            try
            {
                dbSet.Attach(item);
                dbSet.Entry(item).State = EntityState.Modified;

                context.SaveChanges();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                dbSet.Attach(item);
                dbSet.Entry(item).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
