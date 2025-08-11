using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IQuizService : IServiceBase<Quiz, Guid>
    {
        Task<IEnumerable<Quiz>> GetByUserIdAsync(Guid userId);

        Task<Quiz> FinishQuiz(QuizDTO quizDto);
    }
} 