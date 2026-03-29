namespace ivanovGymBackendNetCore.DTO.Auth;

public class SignInResponseDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public UserDTO User { get; set; }
    public DateTime Expires { get; set; }
}