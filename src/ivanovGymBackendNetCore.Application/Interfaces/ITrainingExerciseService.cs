using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface ITrainingExerciseService
{
    Task<List<TrainingExerciseDto>> GetTrainingsByTrainingIdAsync(int id);
    Task<TrainingExerciseDto> CreateTrainingExerciseAsync(TrainingExerciseDto dto);
    Task DeleteTrainingExerciseAsync(int id);
    Task UpdateTrainingExerciseAsync(TrainingExerciseDto dto);
}
