using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class QuizService : ServiceBase<Quiz, Guid>, IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IUnitOfWork unitOfWork, IQuizRepository quizRepository) 
            : base(unitOfWork, quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<IEnumerable<Quiz>> GetByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Quizzes.WhereAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<Quiz>> GetSubjectsByScoreRangeAsync(int minScore, int maxScore)
        {
            return await _unitOfWork.Quizzes.WhereAsync(s => 
                s.Score >= minScore && s.Score <= maxScore);
        }

        public async Task<Quiz> GetSubjectByNameAndUserAsync(string name, Guid userId)
        {
            return await _unitOfWork.Quizzes.FirstOrDefaultAsync(s => 
                s.Name == name && s.UserId == userId);
        }

        public async Task<int> CountSubjectsForUserAsync(Guid userId)
        {
            return await _unitOfWork.Quizzes.CountAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<Quiz>> GetSubjectsWithMoreThanQuestionsAsync(int questionCount)
        {
            return await _unitOfWork.Quizzes.WhereAsync(s => 
                s.QuantityQuestions > questionCount);
        }

        public async Task<Quiz> FinishQuiz(QuizDTO quizDto)
        {
            var quiz = await _unitOfWork.Quizzes.FirstOrDefaultAsync(c => c.Id == quizDto.Id);

            if (quiz == null)
            {
                throw new Exception("Quiz não encontrado");
            }

            quiz.TimesTaken = quizDto.TimesTaken;
            quiz.Score = quizDto.Score ?? 0;
            quiz.LastAttempt = DateTime.UtcNow;
            quiz.TimeSpent = quizDto.TimeSpent;

            await _quizRepository.UpdateAsync(quiz);

            return quiz;
        }
    }
} 