using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IAttemptService : IServiceBase<Attempt, Guid>
    {
        Task<List<Attempt>> GetByQuizIdAsync(Guid QuizId);
    }
}
