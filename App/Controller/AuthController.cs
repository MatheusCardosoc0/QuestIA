using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestIA.Core.Models.DTOs;
using QuestIA.Core.Service;

namespace QuestIA.App.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.ToString() });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDTO>> Register([FromBody] RegisterRequestDTO request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        [HttpPost("revoke")]
        [Authorize]
        public async Task<ActionResult> RevokeToken([FromBody] string token)
        {
            try
            {
                var success = await _authService.RevokeTokenAsync(token);
                if (success)
                {
                    return Ok(new { message = "Token revogado com sucesso" });
                }
                return BadRequest(new { message = "Token inválido ou já revogado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        [HttpGet("validate")]
        [Authorize]
        public async Task<ActionResult> ValidateToken()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token não fornecido" });
                }

                var isValid = await _authService.ValidateTokenAsync(token);
                if (isValid)
                {
                    var user = await _authService.GetUserFromTokenAsync(token);
                    return Ok(new { valid = true, user = user });
                }

                return Unauthorized(new { valid = false, message = "Token inválido" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }
    }
} 