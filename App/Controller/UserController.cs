using Microsoft.AspNetCore.Mvc;
using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Service;
using QuestIA.Core.Repository;

namespace QuestIA.App.Controller
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase<User, UserDTO, Guid>
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserService userService, IUnitOfWork unitOfWork) : base(userService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        // Implementação do mapeamento DTO -> Domain
        protected override User ToDomain(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id ?? Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password ?? "",
                CreatedAt = dto.CreatedAt == DateTime.MinValue ? DateTime.UtcNow : dto.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
        }

        // Implementação do mapeamento Domain -> DTO
        protected override UserDTO ToDto(User entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
} 