namespace ivanovGymBackendNetCore.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Age { get; set; }
    public string? Target { get; set; }
    public string? Experience { get; set; }
    public string? Sleep { get; set; }
    public string? Food { get; set; }
    public string? Pharma { get; set; }
    public string? Activity { get; set; }
    public string? Avatar { get; set; }
    public Guid Guid { get; set; }
    public List<short>? Limits { get; set; }
    public bool IsActive { get; set; }
}
