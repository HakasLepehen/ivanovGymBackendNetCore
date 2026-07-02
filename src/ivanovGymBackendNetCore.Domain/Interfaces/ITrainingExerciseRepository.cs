using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces;

public interface ITrainingExerciseRepository
{
    public Task<List<TrainingExercise>> GetAllAsync(); 
    //public Task<TrainingExercise> CreateAsync();
    //public Task<TrainingExercise> UpdateAsync();
    //public Task DeleteAsync();
}
