using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IQuestionRepository : IRepositoryBase<Question, int>
    {
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId);
    }
}
