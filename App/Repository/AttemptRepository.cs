using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class AttemptRepository : RepositoryBase<Attempt, Guid>, IAttemptRepository
    {
        public AttemptRepository(QuizIAContext context) : base(context)
        {
        }

        public async Task<List<Attempt>> GetByQuizIdAsync(Guid quizId)
        {
            return await _dbSet
                .Where(a => a.QuizId == quizId)
                .Include(a => a.UserResponseQuestions)
                .ToListAsync();
        }
    }
}