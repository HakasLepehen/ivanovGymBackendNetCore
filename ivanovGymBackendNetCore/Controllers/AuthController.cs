using ivanovGymBackendNetCore.DTO.Auth;
using ivanovGymBackendNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignUpRequestDTO request)
    {
        try
        {
            var result = await _authService.SignUpAsync(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }
}