using QuestIA.Core.Models;

namespace QuestIA.Core.Service
{
    public interface IUserService : IServiceBase<User, Guid>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
