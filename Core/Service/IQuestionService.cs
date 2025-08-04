using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IQuestionService : IServiceBase<Question, int>
    {
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId);
        Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<Question>> GenerateQuestionsByQuiz(Guid userId, Guid quizId);
    }
} 