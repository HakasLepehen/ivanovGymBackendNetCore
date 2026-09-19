using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationRequestsController : ControllerBase
    {
        private readonly IConsultationRequestsService _consultationService;
        private readonly ILogger<ConsultationRequestsController> _logger;
        public ConsultationRequestsController(IConsultationRequestsService consultationService, ILogger<ConsultationRequestsController> logger)
        {
            _consultationService = consultationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var requests = await _consultationService.GetRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось получить клиентов");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRequest(CreateConsultationRequestDto dto)
        {
            try
            {
                await _consultationService.CreateRequest(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать запрос на консультацию");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}