using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

class TrainingExerciseRepository : ITrainingExerciseRepository
{
    public Task<List<TrainingExercise>> GetAllAsync()
    {
        return new List<TrainingExercise>();
    }
}