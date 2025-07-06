using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IServiceBase<T, TKey> where T : IEntity<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TKey id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
    }
}
 