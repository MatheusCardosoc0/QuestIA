using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IOptionService : IServiceBase<Option, int>
    {
        Task<IEnumerable<Option>> GetByQuestIdAsync(int questId);
        Task<IEnumerable<Option>> GetByUserIdAsync(Guid userId);
    }
} 