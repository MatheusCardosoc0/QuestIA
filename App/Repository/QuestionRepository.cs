using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class QuestionRepository : RepositoryBase<Question, int>, IQuestionRepository
    {
        public QuestionRepository(QuizIAContext context) : base(context)
        {
        }

        // Métodos específicos do Quest podem ser implementados aqui
        public async Task<IEnumerable<Question>> GetByQuizIdAsync(Guid subjectId)
        {
            return await _dbSet
                .Where(q => q.QuizId == subjectId)
                .Include(q => q.Options)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(q => q.UserId == userId)
                .Include(q => q.Options)
                .ToListAsync();
        }
    }
} 