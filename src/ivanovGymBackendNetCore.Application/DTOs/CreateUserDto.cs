using System.ComponentModel.DataAnnotations;

namespace ivanovGymBackendNetCore.Application.DTOs;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
