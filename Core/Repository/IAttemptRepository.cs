using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IAttemptRepository : IRepositoryBase<Attempt, Guid>
    {
        Task<List<Attempt>> GetByQuizIdAsync(Guid quizId);
    }
}
