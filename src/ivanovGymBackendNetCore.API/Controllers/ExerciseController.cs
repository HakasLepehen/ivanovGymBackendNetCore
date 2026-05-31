using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly ILogger<ExerciseController> _logger;

    public ExerciseController(IExerciseService exerciseService, ILogger<ExerciseController> logger)
    {
        _exerciseService = exerciseService;
        _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Getexercises()
    {
        try
        {
            List<ExerciseDto> res = await _exerciseService.GetExercisesAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting exercises");
            return BadRequest(new {error = ex.Message});
        }
    }
}
