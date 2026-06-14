namespace ivanovGymBackendNetCore.Domain.Entities;

public class Training
{
    public int Id { get; set; }
    public Guid ClientGuid { get; set; }
    public DateTime PlannedDate { get; set; }
    public bool IsCompleted { get; set; } = false;

    public Client Client { get; set; }
}
