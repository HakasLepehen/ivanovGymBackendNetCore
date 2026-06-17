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
    public async Task<IActionResult> CreateTraining()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();

            var model = JsonSerializer.Deserialize<Dictionary<string, object>>(body);


            _logger.LogInformation("Тело запроса CreateTraining: {Body}", body);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось создать тренировку");
            return BadRequest(ex);
        }
    }
}
