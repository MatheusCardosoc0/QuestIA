using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IOptionRepository : IRepositoryBase<Option, int>
    {
        Task<IEnumerable<Option>> GetByQuestIdAsync(int questId);
        Task<IEnumerable<Option>> GetByUserIdAsync(Guid userId);
    }
}
