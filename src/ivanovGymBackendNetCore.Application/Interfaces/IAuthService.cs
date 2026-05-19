using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignUpAsync(string email, string password);
    Task<AuthResultDto> SignInAsync(string email, string password);
}
