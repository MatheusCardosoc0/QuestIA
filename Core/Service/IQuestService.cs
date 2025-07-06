using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IQuestService : IServiceBase<Quest, int>
    {
        Task<IEnumerable<Quest>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<Quest>> GetByUserIdAsync(Guid userId);
    }
} 