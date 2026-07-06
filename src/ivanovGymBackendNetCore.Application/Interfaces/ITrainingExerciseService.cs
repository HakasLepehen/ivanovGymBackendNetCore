using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface ITrainingExerciseService
{
    Task<List<TrainingExerciseDto>> GetTrainingsByTrainingIdAsync(int id);
}
