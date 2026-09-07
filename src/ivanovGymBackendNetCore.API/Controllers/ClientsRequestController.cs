using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsRequestController : ControllerBase
    {
        private readonly IConsultationRequestService consultationService;
        private readonly ILogger<ClientsRequestController> _logger;
        public ClientsRequestController(ILogger<ClientsRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var requests = await consultationService.GetRequests();
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
                await consultationService.CreateRequest(dto);
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