namespace ivanovGymBackendNetCore.Application.DTOs;

public class ExerciseDto
{
    public int? Id { get; set; } = null;
    public string Name { get; set; } = "";
    public string? Url { get; set; } = "";
    public string? Comment { get; set; } = "";
    public int? MuscleGroup { get; set; } = null;
}