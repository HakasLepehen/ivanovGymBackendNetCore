namespace ivanovGymBackendNetCore.Application.DTOs;

public class MemberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime MembershipStartDate { get; set; }
    public DateTime? MembershipEndDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateMemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime MembershipStartDate { get; set; }
}

public class UpdateMemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? MembershipEndDate { get; set; }
    public bool IsActive { get; set; }
}
