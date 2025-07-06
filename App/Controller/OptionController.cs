using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.App.Extensions;
using QuestIA.Core.Models;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class OptionController : ControllerBase<Option, OptionDto, int>
    {
        private readonly IOptionService _optionService;
        private readonly IUnitOfWork _unitOfWork;

        public OptionController(IOptionService optionService, IUnitOfWork unitOfWork) : base(optionService)
        {
            _optionService = optionService;
            _unitOfWork = unitOfWork;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Option ToDomain(OptionDto dto)
        {
            return new Option
            {
                Id = dto.Id ?? 0, // O EF gerará o ID automaticamente
                Description = dto.Description,
                IsCheck = dto.IsCheck,
                QuestId = dto.QuestId ?? 0,
                UserId = dto.UserId ?? Guid.Empty
            };
        }

        // Implementação do mapeamento Domain -> DTO
        protected override OptionDto ToDto(Option entity)
        {
            return new OptionDto
            {
                Id = entity.Id,
                Description = entity.Description,
                IsCheck = entity.IsCheck,
                QuestId = entity.QuestId,
                UserId = entity.UserId
            };
        }

        [Authorize]
        [HttpGet("quest/{questId}")]
        public async Task<ActionResult<IEnumerable<QuestDto>>> GetAllAsync(int questId)
        {
            try
            {
                var userId = this.GetUserId();
                var quests = await _unitOfWork.Options.WhereAsync(c => c.UserId == userId && c.QuestId == questId);
                var questDtos = quests.Select(q => ToDto(q));
                return Ok(questDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        protected override object GetEntityId(Option entity)
        {
            return entity.Id;
        }
    }
} 