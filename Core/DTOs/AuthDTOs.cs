namespace QuestIA.Core.Models.DTOs
{
    // DTO para requisição de login
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // DTO para requisição de registro
    public class RegisterRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    // DTO para resposta de login/registro
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserDTO User { get; set; }
    }

    // DTO para refresh token
    public class RefreshTokenRequestDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    // DTO para revogar token
    public class RevokeTokenRequestDTO
    {
        public string Token { get; set; }
    }

    // DTO para resposta de refresh token
    public class RefreshTokenResponseDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }

    // DTO para reset de senha
    public class ForgotPasswordRequestDTO
    {
        public string Email { get; set; }
    }

    // DTO para confirmação de reset de senha
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
} 