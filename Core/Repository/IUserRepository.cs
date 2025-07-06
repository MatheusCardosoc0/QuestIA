using QuestIA.Core.Models;

namespace QuestIA.Core.Repository
{
    public interface IUserRepository : IRepositoryBase<User, Guid>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
