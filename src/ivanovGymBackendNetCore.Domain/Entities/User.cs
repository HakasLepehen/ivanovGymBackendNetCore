using ivanovGymBackendNetCore.Domain.Enums;

namespace ivanovGymBackendNetCore.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<UserRole> Roles { get; set; } = new() { UserRole.User };
    public string? Token { get; set; }
    public Guid? ClientFkId { get; set; }
}
