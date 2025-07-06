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
    public class QuestController : ControllerBase<Quest, QuestDto, int>
    {
        private readonly IQuestService _questService;
        private readonly IUnitOfWork _unitOfWork;

        public QuestController(IQuestService questService, IUnitOfWork unitOfWork) : base(questService)
        {
            _questService = questService;
            _unitOfWork = unitOfWork;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Quest ToDomain(QuestDto dto)
        {
            return new Quest
            {
                Id = dto.Id ?? 0, // O EF gerará o ID automaticamente
                Question = dto.Question,
                Response = dto.Response,
                SubjectId = dto.SubjectId ?? Guid.Empty,
                UserId = dto.UserId ?? Guid.Empty
            };
        }

        // Implementação do mapeamento Domain -> DTO
        protected override QuestDto ToDto(Quest entity)
        {
            return new QuestDto
            {
                Id = entity.Id,
                Question = entity.Question,
                Response = entity.Response,
                SubjectId = entity.SubjectId,
                UserId = entity.UserId
            };
        }

        [Authorize]
        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<QuestDto>>> GetAllAsync(Guid subjectId)
        {
            try
            {
                var userId = this.GetUserId();
                var quests = await _unitOfWork.Quests.WhereAsync(c => c.UserId == userId && c.SubjectId == subjectId);
                var questDtos = quests.Select(q => ToDto(q));
                return Ok(questDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        protected override object GetEntityId(Quest entity)
        {
            return entity.Id;
        }
    }
} 