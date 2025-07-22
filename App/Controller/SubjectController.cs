using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.App.Extensions;
using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase<Subject, SubjectDTO, Guid>
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService) : base(subjectService)
        {
            _subjectService = subjectService;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override Subject ToDomain(SubjectDTO dto)
        {
            return new Subject
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Score = dto.Score,
                TimeLimit = dto.TimeLimit,
                TimesTaken = dto.TimesTaken,
                QuantityQuests = dto.QuantityQuests,
                AutoSubmitOnTimeout = dto.AutoSubmitOnTimeout,
                DifficultyLevel = dto.DifficultyLevel,
                IsPublic = dto.IsPublic,
                IsRandom = dto.IsRandom,
                QuestType = dto.QuestType,
                Tags = dto.Tags ?? new List<string>(),
                UserId = dto.UserId ?? Guid.Empty
            };
        }

        protected override SubjectDTO ToDto(Subject entity)
        {
            return new SubjectDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Score = entity.Score,
                TimeLimit = entity.TimeLimit,
                TimesTaken = entity.TimesTaken,
                QuantityQuests = entity.QuantityQuests,
                AutoSubmitOnTimeout = entity.AutoSubmitOnTimeout,
                DifficultyLevel = entity.DifficultyLevel,
                IsPublic = entity.IsPublic,
                IsRandom = entity.IsRandom,
                QuestType = entity.QuestType,
                Tags = entity.Tags,
                UserId = entity.UserId
            };
        }

        //[Authorize]
        //[HttpGet]
        //public override async Task<ActionResult<IEnumerable<SubjectDTO>>> GetAllAsync()
        //{
        //    try
        //    {
        //        var userId = this.GetUserId();
        //        var subjects = await _subjectService.GetByUserIdAsync(userId);
        //        var subjectDtos = subjects.Select(s => ToDto(s));
        //        return Ok(subjectDtos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        //    }
        //}

        protected override object GetEntityId(Subject entity)
        {
            return entity.Id;
        }
    }
} 