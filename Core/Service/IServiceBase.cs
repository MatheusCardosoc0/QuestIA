using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IServiceBase<T, TKey> where T : IEntity<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync(Guid userId);
        Task<T> GetByIdAsync(TKey id, Guid userId);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity, Guid userId);
        Task DeleteAsync(TKey id, Guid userId);
    }
}
 