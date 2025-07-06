using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class QuestService : ServiceBase<Quest, int>, IQuestService
    {
        private readonly IQuestRepository _questRepository;

        public QuestService(IUnitOfWork unitOfWork, IQuestRepository questRepository) 
            : base(unitOfWork, questRepository)
        {
            _questRepository = questRepository;
        }

        public async Task<IEnumerable<Quest>> GetBySubjectIdAsync(Guid subjectId)
        {
            return await _questRepository.GetBySubjectIdAsync(subjectId);
        }

        public async Task<IEnumerable<Quest>> GetByUserIdAsync(Guid userId)
        {
            return await _questRepository.GetByUserIdAsync(userId);
        }
    }
} 