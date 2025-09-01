using Microsoft.AspNetCore.Mvc;
using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Service;
using QuestIA.Core.Repository;
using QuestIA.App.Extensions;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class AttemptController : ControllerBase<Attempt, AttemptDTO, Guid>
    {
        private readonly IAttemptService _attemptService;
        private readonly IUnitOfWork _unitOfWork;

        public AttemptController(IAttemptService attemptService, IUnitOfWork unitOfWork) : base(attemptService)
        {
            _attemptService = attemptService;
            _unitOfWork = unitOfWork;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Attempt ToDomain(AttemptDTO dto)
        {
            return new Attempt
            {
                Id = dto.Id ?? Guid.NewGuid(),
                QuizId = dto.QuizId,
                UserResponseQuestions = dto.UserResponseQuestions ?? new List<UserResponseQuestion>(),
                UserId = dto.UserId
            };
        }

        // Implementação do mapeamento Domain -> DTO
        protected override AttemptDTO ToDto(Attempt entity)
        {
            return new AttemptDTO
            {
                Id = entity.Id,
                QuizId = entity.QuizId,
                UserResponseQuestions = entity.UserResponseQuestions,
                UserId = entity.UserId
            };
        }

        // Método específico para buscar tentativas por Quiz ID
        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<List<AttemptDTO>>> GetByQuizIdAsync(Guid quizId)
        {
            try
            {
                var attempts = await _attemptService.GetByQuizIdAsync(quizId);
                var dtos = attempts.Select(a => ToDto(a)).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }
    }
} 