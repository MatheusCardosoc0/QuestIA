using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;

namespace QuestIA.Core.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<LoginResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<LoginResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDTO> GetUserFromTokenAsync(string token);
    }
} 