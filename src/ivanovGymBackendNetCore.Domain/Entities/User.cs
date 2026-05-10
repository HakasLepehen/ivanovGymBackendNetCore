using ivanovGymBackendNetCore.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ivanovGymBackendNetCore.Domain.Entities;

public class User : IdentityUser
{
    public List<UserRole> Roles { get; set; } = new() { UserRole.User };
    public string? Token { get; set; }
    public Guid? ClientFkId { get; set; }
}
