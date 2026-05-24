using System.Security.Claims;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;
  private readonly ILogger<AuthController> _logger;

  public AuthController(
      IAuthService authService,
      ILogger<AuthController> logger)
  {
    _authService = authService;
    _logger = logger;
  }

  /// <summary>
  /// Регистрация нового пользователя
  /// </summary>
  [HttpPost("signup")]
  public async Task<IActionResult> SignUp([FromBody] RegisterDto model)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
      {
        return BadRequest(new { error = "Email and password are required" });
      }

      string token = await _authService.SignUpAsync(model.Email, model.Password);
      return Ok(new { token, message = "Пользователь успешно зарегистрирован" });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Registration failed for email: {Email}", model.Email);
      return BadRequest(new { error = ex.Message });
    }
  }

  /// <summary>
  /// Вход пользователя
  /// </summary>
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDto model)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
      {
        return BadRequest(new { error = "Email and password are required" });
      }

      var result = await _authService.SignInAsync(model.Email, model.Password);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Login failed for email: {Email}", model.Email);
      return BadRequest(new { error = ex.Message });
    }
  }

  /// <summary>
  /// Получение информации о текущем пользователе
  /// </summary>
  [HttpGet("me")]
  [Authorize]
  public IActionResult GetCurrentUser()
  {
    string email = User.FindFirst(ClaimTypes.Email)?.Value;
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

    return Ok(new
    {
      Email = email,
      UserId = userId,
      Roles = roles
    });
  }
}
