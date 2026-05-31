using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces;

public interface IExerciseRepository
{
    Task<List<Exercise>> GetAllAsync();
    Task<Exercise?> GetByIdAsync(int Id);
    Task<Exercise> CreateAsync(Exercise model);
    Task<Exercise> UpdateAsync(Exercise model);
    Task DeleteAsync(int id);
}
