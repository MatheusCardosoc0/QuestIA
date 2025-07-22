using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.App.Extensions;
using QuestIA.Core.Models;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ControllerBase<T, TDto, TKey> : Microsoft.AspNetCore.Mvc.ControllerBase where T : IEntity<TKey> where TDto : class
    {
        protected readonly IServiceBase<T, TKey> _service;

        public ControllerBase(IServiceBase<T, TKey> service)
        {
            _service = service;
        }

        // Métodos virtuais para mapeamento - devem ser sobrescritos nos controllers específicos
        protected virtual T ToDomain(TDto dto)
        {
            throw new NotImplementedException("Método ToDomain deve ser implementado no controller específico");
        }

        protected virtual TDto ToDto(T entity)
        {
            throw new NotImplementedException("Método ToDto deve ser implementado no controller específico");
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var userId = this.GetUserId();

                var entities = await _service.GetAllAsync(userId);
                var dtos = entities.Select(e => ToDto(e));
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> GetByIdAsync(TKey id)
        {
            try
            {
                var userId = this.GetUserId();

                var entity = await _service.GetByIdAsync(id, userId);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(ToDto(entity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<TDto>> CreateAsync([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("DTO não pode ser nulo");
                }

                var userId = this.GetUserId();

                var entity = ToDomain(dto);

                entity.UserId = userId;

                var createdEntity = await _service.CreateAsync(entity);
                var createdDto = ToDto(createdEntity);

                return Ok(dto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TDto>> UpdateAsync(TKey id, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("DTO não pode ser nulo");
                }

                var userId = this.GetUserId();

                var existingEntity = await _service.GetByIdAsync(id, userId);
                if (existingEntity == null)
                {
                    return NotFound();
                }

                existingEntity.Id = id;
                var updatedEntity = await _service.UpdateAsync(existingEntity, userId);
                return Ok(ToDto(updatedEntity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(TKey id)
        {
            try
            {
                var userId = this.GetUserId();

                await _service.DeleteAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.ToString()}");
            }
        }

        protected virtual object GetEntityId(T entity)
        {
            // Método virtual para obter o ID da entidade
            // Pode ser sobrescrito em controllers específicos se necessário
            var idProperty = typeof(T).GetProperty("Id");
            return idProperty?.GetValue(entity);
        }
    }
}