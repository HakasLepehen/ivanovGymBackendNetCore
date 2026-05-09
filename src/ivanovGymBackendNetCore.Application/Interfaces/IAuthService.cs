namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignUpAsync(string email, string password);
}
