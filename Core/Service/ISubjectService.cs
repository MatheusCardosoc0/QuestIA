using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface ISubjectService : IServiceBase<Subject, Guid>
    {
        Task<IEnumerable<Subject>> GetByUserIdAsync(Guid userId);
    }
} 