using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.App.Extensions;
using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class QuizController : ControllerBase<Quiz, QuizDTO, Guid>
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService) : base(quizService)
        {
            _quizService = quizService;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Quiz ToDomain(QuizDTO dto)
        {
            return new Quiz
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Score = dto.Score ?? 0,
                TimeLimit = dto.TimeLimit,
                TimesTaken = dto.TimesTaken,
                QuantityQuestions = dto.QuantityQuestions,
                AutoSubmitOnTimeout = dto.AutoSubmitOnTimeout,
                DifficultyLevel = dto.DifficultyLevel,
                IsPublic = dto.IsPublic,
                IsRandom = dto.IsRandom,
                QuestionType = dto.QuestionTypes,
                Tags = dto.Tags ?? new List<string>(),
                UserId = dto.UserId ?? Guid.Empty
            };
        }

        protected override QuizDTO ToDto(Quiz entity)
        {
            return new QuizDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Score = (double)Math.Round(entity.Score,2),
                TimeLimit = entity.TimeLimit,
                TimesTaken = entity.TimesTaken,
                QuantityQuestions = entity.QuantityQuestions,
                AutoSubmitOnTimeout = entity.AutoSubmitOnTimeout,
                DifficultyLevel = entity.DifficultyLevel,
                IsPublic = entity.IsPublic,
                IsRandom = entity.IsRandom,
                QuestionTypes = entity.QuestionType,
                TimeSpent = entity.TimeSpent,
                Tags = entity.Tags,
                UserId = entity.UserId
            };
        }

        [HttpPut("finish/{id}")]
        public virtual async Task<ActionResult<QuizDTO>> FinishQuiz(Guid id, [FromBody] QuizDTO dto)
        {
            try
            {
                dto.Id = id;
                var updatedEntity = await _quizService.FinishQuiz(dto);
                return Ok(ToDto(updatedEntity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

   
    }
} 