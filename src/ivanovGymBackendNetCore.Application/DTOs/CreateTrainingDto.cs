namespace ivanovGymBackendNetCore.Application.DTOs;

public class CreateTrainingDto
{
    public Guid ClientGuid { get; set; }
    public DateTime PlannedDate { get; set; }
}
