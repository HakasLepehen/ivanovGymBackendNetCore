using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IExerciseService
{
    Task<List<ExerciseDto>> GetExercisesAsync();
    Task AddExerciseAsync(CreateExerciseDto dto);
}
