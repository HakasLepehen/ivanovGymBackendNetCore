using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/training_exercises")]
public class TrainingExerciseController : ControllerBase
{
    private readonly ILogger<TrainingExerciseController> _logger;
    private readonly ITrainingExerciseService _service;

    public TrainingExerciseController(ITrainingExerciseService trExService, ILogger<TrainingExerciseController> logger)
    {
        _logger = logger;
        _service = trExService;
    }

    /// <summary>
    /// Получение списка упражнений для конкретной тренировки
    /// </summary>
    /// <param name="id">Идентификатор тренировки</param>
    /// <returns>Список упражнений</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingExercises(int id)
    {
        try
        {
            var res = await _service.GetTrainingsByTrainingIdAsync(id);
            return Ok(res);
        } 
        catch(Exception ex)
        {
            return BadRequest(ex);
        }

    }

    // пока не используется
    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> CreateOrUpdateTrainingExersises(int id, [FromBody] List<TrainingExerciseDto> exercises)
    { 
        try
        {
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
