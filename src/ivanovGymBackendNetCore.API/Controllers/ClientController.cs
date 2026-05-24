using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ivanovGymBackendNetCore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IClientService clientService, ILogger<ClientController> logger)
    {
        _clientService = clientService;
        _logger = logger;

    }

    /// <summary>
    /// Получение списка клиентов
    /// </summary>
    /// <returns></returns>ad
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        try
        {
            List<ClientDto> res = await _clientService.GetClientsAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось получить клиентов");
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось удалить клиента");
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateClient(ClientDto client)
    {
        try
        {
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось отредактировать клиента клиента");
            return BadRequest(new { error = ex.Message });
        }
    }
}
