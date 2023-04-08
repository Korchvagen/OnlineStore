using OnlineStore.DAL.Context;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly OnlineStoreContext _context;

        public BaseRepository(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public async Task<bool> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
