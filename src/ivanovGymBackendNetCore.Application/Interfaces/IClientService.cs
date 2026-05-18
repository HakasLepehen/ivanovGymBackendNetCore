using System.ComponentModel.DataAnnotations;
using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllClientsAsync();
    // Task<MemberDto?> GetClientByIdAsync(Guid id);
    Task<ClientDto> CreateClientAsync(CreateClientDto dto);
    // Task<MemberDto?> UpdateClientAsync(Guid id, UpdateMemberDto dto);
    Task DeleteClientAsync(Guid id);
    Task<ClientDto> GetClientByEmailAsync(string email);
}
