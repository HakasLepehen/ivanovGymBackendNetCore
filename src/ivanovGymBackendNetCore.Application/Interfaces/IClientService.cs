using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllClientsAsync();
    // Task<MemberDto?> GetClientByIdAsync(Guid id);
    // Task<MemberDto> CreateClientAsync(CreateMemberDto dto);
    // Task<MemberDto?> UpdateClientAsync(Guid id, UpdateMemberDto dto);
    Task DeleteClientAsync(Guid id);
}
