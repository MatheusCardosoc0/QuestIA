using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;
using System.Linq.Expressions;

namespace QuestIA.App.Repository
{
    public class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T : class, IEntity<TKey>
    {
        protected readonly QuestIAContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(QuestIAContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        // Implementação dos métodos com expressões LINQ
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
} 