using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class UserService : ServiceBase<User, Guid>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository) 
            : base(unitOfWork, userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }
    }
} 