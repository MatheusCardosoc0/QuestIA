using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.App.Extensions;
using QuestIA.App.Service;
using QuestIA.Core.Models;
using QuestIA.Core.Models;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase<Question, QuestionDto, int>
    {
        private readonly IQuestionService _questService;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionController(IQuestionService questService, IUnitOfWork unitOfWork) : base(questService)
        {
            _questService = questService;
            _unitOfWork = unitOfWork;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Question ToDomain(QuestionDto dto)
        {
            return new Question
            {
                Id = dto.Id ?? 0, // O EF gerará o ID automaticamente
                QuestionText = dto.QuestionText,
                Response = dto.Response,
                QuizId = dto.QuizId ?? Guid.Empty,
                UserId = dto.UserId ?? Guid.Empty,
                Options = dto.Options,
            };
        }

        // Implementação do mapeamento Domain -> DTO
        protected override QuestionDto ToDto(Question entity)
        {
            return new QuestionDto
            {
                Id = entity.Id,
                QuestionText = entity.QuestionText,
                Response = entity.Response,
                QuizId = entity.QuizId,
                UserId = entity.UserId,
                Options = entity.Options,
            };
        }

        [Authorize]
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllAsync(Guid quizId)
        {
            try
            {
                var userId = this.GetUserId();
                var quests = await _unitOfWork.Questions.WhereAsync(c => c.UserId == userId && c.QuizId == quizId);
                var questDtos = quests.Select(q => ToDto(q));
                return Ok(questDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("generate-by-quiz/{quizId}")]
        public virtual async Task<ActionResult<QuestionDto>> GenerateQuestionsByQuiz(Guid quizId)
        {
            try
            {

                var userId = this.GetUserId();

                var createdEntity = await _questService.GenerateQuestionsByQuiz(userId, quizId);
                var createdDto = createdEntity.Select(ToDto);

                return Ok(createdDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        protected override object GetEntityId(Question entity)
        {
            return entity.Id;
        }
    }
} 