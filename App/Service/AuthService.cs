using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuestIA.Core.Models;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Repository;
using QuestIA.Core.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuestIA.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
            _userService = userService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);
            
            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas");
            }

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                User = MapToUserDto(user)
            };
        }

        public async Task<LoginResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("As senhas não coincidem");
            }

            if (await _userService.EmailExistsAsync(request.Email))
            {
                throw new ArgumentException("E-mail já cadastrado");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userService.CreateAsync(user);

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new LoginResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                User = MapToUserDto(user)
            };
        }

        public async Task<LoginResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var refreshToken = await GetRefreshTokenAsync(request.RefreshToken);
            
            if (refreshToken == null || !refreshToken.IsActive || refreshToken.UserId != userId)
            {
                throw new UnauthorizedAccessException("Token de atualização inválido");
            }

            var user = await _userService.GetByIdAsync(userId);
            
            // Revoga o refresh token atual
            refreshToken.IsRevoked = true;
            await UpdateRefreshTokenAsync(refreshToken);

            // Gera novos tokens
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new LoginResponseDTO
            {
                Token = newToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                User = MapToUserDto(user)
            };
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var refreshToken = await GetRefreshTokenAsync(token);
            
            if (refreshToken == null || !refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.IsRevoked = true;
            await UpdateRefreshTokenAsync(refreshToken);
            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDTO> GetUserFromTokenAsync(string token)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            return MapToUserDto(await _userService.GetByIdAsync(userId));
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
        {
            // Remove tokens expirados ou revogados do usuário
            await RemoveExpiredRefreshTokensAsync(userId);

            RefreshToken returnRefresh;

            // Salva o refresh token no banco usando UnitOfWork
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var verifyRefreshToken = await _unitOfWork.RefreshToken.FirstOrDefaultAsync(c => c.UserId == userId);
                if(verifyRefreshToken != null)
                {
                    verifyRefreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                    verifyRefreshToken.ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                    verifyRefreshToken.CreatedAt = DateTime.UtcNow;

                    await _unitOfWork.RefreshToken.UpdateAsync(verifyRefreshToken);

                    returnRefresh = verifyRefreshToken;
                } else
                {
                    var refreshToken = new RefreshToken
                    {
                        Id = Guid.NewGuid(),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                        ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                        CreatedAt = DateTime.UtcNow,
                        UserId = userId
                    };

                    await _unitOfWork.RefreshToken.CreateAsync(refreshToken);

                    returnRefresh = refreshToken;
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return returnRefresh;
        }

        private async Task RemoveExpiredRefreshTokensAsync(Guid userId)
        {
            var expiredTokens = await _unitOfWork.RefreshToken.WhereAsync(rt => 
                rt.UserId == userId && (rt.IsRevoked || rt.ExpiresAt <= DateTime.UtcNow));

            if (expiredTokens.Any())
            {
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    foreach (var token in expiredTokens)
                    {
                        await _unitOfWork.RefreshToken.DeleteAsync(token.Id);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false, // Permite tokens expirados
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }

        private async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _unitOfWork.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        private async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.RefreshToken.UpdateAsync(refreshToken);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private UserDTO MapToUserDto(User user)
        {
            return new UserDTO
            {
                Id = null,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }

    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
} 