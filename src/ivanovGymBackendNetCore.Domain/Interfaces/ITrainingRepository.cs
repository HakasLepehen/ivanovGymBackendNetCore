using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Domain.Interfaces;

public interface ITrainingRepository
{
    public Task<List<Training>> GetAllAsync();
    public Task<Training?> GetByIdAsync(int Id);
    public Task<Training> CreateAsync(Training model);
    public Task<Training> UpdateAsync(Training model);
    public Task DeleteAsync(int id);
}
