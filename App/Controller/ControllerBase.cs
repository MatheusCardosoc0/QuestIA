using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                var entities = await _service.GetAllAsync();
                var dtos = entities.Select(e => ToDto(e));
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> GetByIdAsync(TKey id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(ToDto(entity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
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

                var entity = ToDomain(dto);
                var createdEntity = await _service.CreateAsync(entity);
                var createdDto = ToDto(createdEntity);
                
                return CreatedAtAction(nameof(GetByIdAsync), new { id = GetEntityId(createdEntity) }, createdDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
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

                var existingEntity = await _service.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    return NotFound();
                }

                existingEntity.Id = id;
                var updatedEntity = await _service.UpdateAsync(existingEntity);
                return Ok(ToDto(updatedEntity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(TKey id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
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