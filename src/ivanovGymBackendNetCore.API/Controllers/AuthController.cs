using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost]
  public async Task<IActionResult> Signup(CreateUserDto user)
  {
    try
    {
      
    }
    catch (Exception ex)
    {
      return BadRequest(ex);
    }
  }
}
