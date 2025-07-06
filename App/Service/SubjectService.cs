using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class SubjectService : ServiceBase<Subject, Guid>, ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(IUnitOfWork unitOfWork, ISubjectRepository subjectRepository) 
            : base(unitOfWork, subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<Subject>> GetByUserIdAsync(Guid userId)
        {
            // Usando expressão LINQ diretamente
            return await _unitOfWork.Subjects.WhereAsync(s => s.UserId == userId);
        }

        // Exemplos de métodos usando as novas capacidades LINQ
        public async Task<IEnumerable<Subject>> GetSubjectsByScoreRangeAsync(int minScore, int maxScore)
        {
            return await _unitOfWork.Subjects.WhereAsync(s => 
                s.Score >= minScore && s.Score <= maxScore);
        }

        public async Task<Subject> GetSubjectByNameAndUserAsync(string name, Guid userId)
        {
            return await _unitOfWork.Subjects.FirstOrDefaultAsync(s => 
                s.Name == name && s.UserId == userId);
        }

        public async Task<int> CountSubjectsForUserAsync(Guid userId)
        {
            return await _unitOfWork.Subjects.CountAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsWithMoreThanQuestionsAsync(int questionCount)
        {
            return await _unitOfWork.Subjects.WhereAsync(s => 
                s.QuantityQuests > questionCount);
        }
    }
} 