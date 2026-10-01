using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces;

public interface ITrainingExerciseRepository
{
    public Task<List<TrainingExercise>> GetAllAsync(); 
    public Task<List<TrainingExercise>> GetAllByTrainingIdAsync(int id); 
    public Task<TrainingExercise> CreateExerciseAsync(TrainingExercise dto);
    public Task UpdateExerciseAsync(TrainingExercise model);
    public Task DeleteAsync(int id);
}
