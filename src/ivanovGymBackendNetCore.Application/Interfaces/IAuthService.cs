namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignUpAsync(string username, string password);
}
