using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class QuestRepository : RepositoryBase<Quest, int>, IQuestRepository
    {
        public QuestRepository(QuestIAContext context) : base(context)
        {
        }

        // Métodos específicos do Quest podem ser implementados aqui
        public async Task<IEnumerable<Quest>> GetBySubjectIdAsync(Guid subjectId)
        {
            return await _dbSet
                .Where(q => q.SubjectId == subjectId)
                .Include(q => q.Options)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quest>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(q => q.UserId == userId)
                .Include(q => q.Options)
                .ToListAsync();
        }
    }
} 