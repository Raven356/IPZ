using BlogMVC.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IBlogMVCContext _context;
        private bool disposedValue;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IBlogMVCContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var created = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return created.Entity;
        }

        public async Task Delete(int? id)
        {
            var entity = (await _dbSet.FindAsync(id))!;
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity!);
            }
            _dbSet.Remove(entity!);

            await _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public async Task<TEntity> GetById(object id)
        {
            return (await _dbSet.FindAsync(id))!;
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
