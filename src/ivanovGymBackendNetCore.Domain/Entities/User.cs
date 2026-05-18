using ivanovGymBackendNetCore.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ivanovGymBackendNetCore.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string[] Roles { get; set; } = new[] { "user" };
    public string? Token { get; set; }
    public int? ClientFkId { get; set; }
}
