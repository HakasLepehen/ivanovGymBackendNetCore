using Microsoft.AspNetCore.Identity;

namespace ivanovGymBackendNetCore.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string[] Roles { get; set; }
    public string? Token { get; set; }
    public int? ClientFkId { get; set; }
}
