using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IQuestRepository : IRepositoryBase<Quest, int>
    {
        Task<IEnumerable<Quest>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<Quest>> GetByUserIdAsync(Guid userId);
    }
}
