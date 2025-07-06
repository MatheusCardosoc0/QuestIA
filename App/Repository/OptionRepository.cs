using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class OptionRepository : RepositoryBase<Option, int>, IOptionRepository
    {
        public OptionRepository(QuestIAContext context) : base(context)
        {
        }

        // Métodos específicos do Option podem ser implementados aqui
        public async Task<IEnumerable<Option>> GetByQuestIdAsync(int questId)
        {
            return await _dbSet
                .Where(o => o.QuestId == questId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Option>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
} 