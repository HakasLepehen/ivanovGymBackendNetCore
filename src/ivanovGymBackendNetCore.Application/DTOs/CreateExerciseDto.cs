namespace ivanovGymBackendNetCore.Application.DTOs;

public class CreateExerciseDto
{
    public string Name { get; set; } = "";
    public string? Url { get; set; } = "";
    public string? Comment { get; set; } = "";
    public int MuscleGroup { get; set; }
}
