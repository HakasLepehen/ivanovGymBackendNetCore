namespace ivanovGymBackendNetCore.Domain.Entities;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Url { get; set; }
    public string? Comment { get; set; }
    public int MuscleGroup { get; set; }
}
