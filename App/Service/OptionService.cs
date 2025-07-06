using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Service
{
    public class OptionService : ServiceBase<Option, int>, IOptionService
    {
        private readonly IOptionRepository _optionRepository;

        public OptionService(IUnitOfWork unitOfWork, IOptionRepository optionRepository) 
            : base(unitOfWork, optionRepository)
        {
            _optionRepository = optionRepository;
        }

        public async Task<IEnumerable<Option>> GetByQuestIdAsync(int questId)
        {
            // Usando expressão LINQ diretamente
            return await _unitOfWork.Options.WhereAsync(o => o.QuestId == questId);
        }

        public async Task<IEnumerable<Option>> GetByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Options.WhereAsync(o => o.UserId == userId);
        }

        // Exemplos de métodos usando as novas capacidades LINQ
        public async Task<IEnumerable<Option>> GetCheckedOptionsAsync()
        {
            return await _unitOfWork.Options.WhereAsync(o => o.IsCheck == true);
        }

        public async Task<IEnumerable<Option>> GetUncheckedOptionsAsync()
        {
            return await _unitOfWork.Options.WhereAsync(o => o.IsCheck == false);
        }

        public async Task<Option> GetOptionByDescriptionAsync(string description)
        {
            return await _unitOfWork.Options.FirstOrDefaultAsync(o => 
                o.Description == description);
        }

        public async Task<int> CountOptionsByQuestAsync(int questId)
        {
            return await _unitOfWork.Options.CountAsync(o => o.QuestId == questId);
        }

        public async Task<IEnumerable<Option>> GetOptionsByDescriptionContainsAsync(string text)
        {
            return await _unitOfWork.Options.WhereAsync(o => 
                o.Description.Contains(text));
        }

        public async Task<IEnumerable<Option>> GetCheckedOptionsByUserAsync(Guid userId)
        {
            return await _unitOfWork.Options.WhereAsync(o => 
                o.UserId == userId && o.IsCheck == true);
        }

        // Exemplo de query complexa usando AsQueryable
        public async Task<IEnumerable<Option>> GetOptionsWithLongDescriptionsAsync(int minLength)
        {
            return await Task.FromResult(
                _unitOfWork.Options.AsQueryable()
                    .Where(o => o.Description.Length > minLength)
                    .OrderBy(o => o.Description)
                    .ToList()
            );
        }
    }
} 