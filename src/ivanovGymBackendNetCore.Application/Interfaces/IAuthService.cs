using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignUpAsync(string email, string password);
    Task<string> SignInAsync(string email, string password);
    Task<AuthResultDto> AuthenticateAsync(string email, string password);
}
