using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/training_exercises")]
[Authorize]
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingExercise(int id)
    {
        try
        {
            await _service.DeleteTrainingExerciseAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("getLastExercise/{id}")]
    public async Task<IActionResult> GetLastExercise(int id, [FromBody] LastExerciseDto dto, [FromQuery] bool isRepetition)
    {
        try
        {
            var res = await _service.FindLastExerciseAsync(id, dto);
            return Ok(res);
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }
    }
}
