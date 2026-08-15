using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
  /// <summary>
  /// Проверка доступности API
  /// </summary>
  [HttpGet]
  public IActionResult Get()
  {
    return Ok("Hello world");
  }
}
