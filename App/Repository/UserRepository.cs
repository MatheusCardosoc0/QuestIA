using Microsoft.EntityFrameworkCore;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Infra.Database;

namespace QuestIA.App.Repository
{
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        public UserRepository(QuestIAContext context) : base(context)
        {
        }

        // Métodos específicos do User podem ser implementados aqui
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
} 