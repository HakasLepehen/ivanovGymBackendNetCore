using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingExerciseController : ControllerBase
{
    private readonly ILogger<TrainingExerciseController> _logger;

    public TrainingExerciseController(ILogger<TrainingExerciseController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetTrainingExercises(int id)
    {
        return Ok();
    }
}
