using System.Linq.Expressions;
using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IRepositoryBase<T, Tkey> where T : class, IEntity<Tkey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Tkey id);                   
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Tkey id);
        Task<bool> ExistsAsync(Tkey id);

        // Métodos com expressões LINQ
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        
        // Métodos síncronos para compatibilidade
        IQueryable<T> AsQueryable();
    }
}
