using System.Text.Json;
using System.Text.Json.Serialization;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainingsController : ControllerBase
{
    private readonly ITrainingService _trainingService;
    private readonly ILogger<TrainingsController> _logger;

    public TrainingsController(ITrainingService trainingService, ILogger<TrainingsController> logger)
    {
        _trainingService = trainingService;
        _logger = logger;
    }

    /// <summary>
    /// Получение списка тренировок
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetTrainings()
    {
        try
        {
            List<TrainingDto> res = await _trainingService.GetTrainingsAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось получить список тренировок");
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Получение конкретной тренировки тренировок
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTraining(int id)
    {
        try
        {
            TrainingDto res = await _trainingService.GetTrainingAsync(id);
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось получить список тренировок");
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTraining([FromBody] CreateTrainingDto dto)
    {
        try
        {
            TrainingDto res = await _trainingService.CreateTrainingAsync(dto);

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось создать тренировку");
            return BadRequest(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTraining(int id)
    {
        try
        {
            await _trainingService.DeleteTrainingAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось удалить тренировку");
            return BadRequest(new {error = ex.Message});
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTraining(int id, [FromBody] TrainingDto dto)
    {
        try
        {
            await _trainingService.UpdateTrainingAsync(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка обновления тренировки {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("copyTraining/{id}")]
    public async Task<IActionResult> CopyTraining(int id, [FromBody] TrainingDto dto)
    {
        try
        {
            CreateTrainingDto creationTraining = new CreateTrainingDto()
            {
                ClientGuid = dto.ClientGuid,
                PlannedDate = dto.PlannedDate,
            };

            TrainingDto resultTraining = await _trainingService.CreateTrainingAsync(creationTraining);

            await _trainingService.CopyTrainingExercisesAsync((int)resultTraining.Id, dto.Exercises);


            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Ошибка копирования тренировки {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }
}
