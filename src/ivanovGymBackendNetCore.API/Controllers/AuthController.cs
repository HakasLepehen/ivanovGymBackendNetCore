using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
  public async Task<IActionResult> Signup(string email, string password)
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
