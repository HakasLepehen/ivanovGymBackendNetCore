using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public Task DeleteClientAsync(Guid id) => throw new NotImplementedException();
    public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ClientDto>>(clients);
    }

    public async Task<ClientDto> GetClientByEmailAsync(string email)
    {
        var client = await _clientRepository.GetByEmailAsync(email);
        return _mapper.Map<ClientDto>(client);
    }

    public async Task<MemberDto> CreateClientAsync(CreateClientDto dto)
    {
        var client = _mapper.Map<Client>(dto);
        client.IsActive = true;

        var createdClient = await _clientRepository.CreateAsync(client);
        return _mapper.Map<MemberDto>(createdClient);
    }
}
