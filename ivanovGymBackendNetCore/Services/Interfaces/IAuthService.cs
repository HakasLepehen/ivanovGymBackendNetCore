using ivanovGymBackendNetCore.DTO.Auth;

namespace ivanovGymBackendNetCore.Services.Interfaces;

public interface IAuthService
{
    Task<UserDTO> SignUpAsync(SignUpRequestDTO model);
}