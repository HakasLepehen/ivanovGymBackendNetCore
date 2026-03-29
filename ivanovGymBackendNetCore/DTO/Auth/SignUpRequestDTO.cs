using System.ComponentModel.DataAnnotations;

namespace ivanovGymBackendNetCore.DTO.Auth;

public class SignUpRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}