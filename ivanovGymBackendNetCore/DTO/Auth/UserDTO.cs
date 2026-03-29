namespace ivanovGymBackendNetCore.DTO.Auth;

public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsAdmin { get; set; }
}