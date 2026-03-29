using System.ComponentModel.DataAnnotations;

namespace ivanovGymBackendNetCore.DTO.Auth;

public class SignInRequestDTO
{
    [Required]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
}