using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class AttemptService : ServiceBase<Attempt, Guid>, IAttemptService
    {
        private readonly IAttemptRepository _attemptRepository;

        public AttemptService(IUnitOfWork unitOfWork, IAttemptRepository attemptRepository) 
            : base(unitOfWork, attemptRepository)
        {
            _attemptRepository = attemptRepository;
        }

        public async Task<List<Attempt>> GetByQuizIdAsync(Guid quizId)
        {
            return await _attemptRepository.GetByQuizIdAsync(quizId);
        }

        public override async Task<Attempt> GetByIdAsync(Guid id, Guid userId)
        {
            return await _attemptRepository.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
} 