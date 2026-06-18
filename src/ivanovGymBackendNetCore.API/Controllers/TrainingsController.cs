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
    [Authorize]
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTraining([FromBody] CreateTrainingDto dto)
    {
        try
        {
            TrainingDto res = await _trainingService.CreateTraining(dto);

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось создать тренировку");
            return BadRequest(ex);
        }
    }
}
