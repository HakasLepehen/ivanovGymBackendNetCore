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

    /// <summary>
    /// Получение списка упражнений
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetExercises()
    {
        try
        {
            List<ExerciseDto> res = await _exerciseService.GetExercisesAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting exercises");
            return BadRequest(new { error = ex.Message });
        }
    }
    /// <summary>
    /// добавление упражнения
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddExercise(CreateExerciseDto dto)
    {
        try
        {
            await _exerciseService.AddExerciseAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding exercise");
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        try
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting exercise");
            return BadRequest(new { error = ex.Message });
        }
    }
}
