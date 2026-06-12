namespace ivanovGymBackendNetCore.Domain.Entities;

public class Training
{
    public int Id { get; set; }
    public Guid ClientGuid { get; set; }
    public byte Hour { get; set; }
    public byte Minutes { get; set; }
    public DateTime PlannedDate { get; set; }
    public bool IsCompleted { get; set; }
}
