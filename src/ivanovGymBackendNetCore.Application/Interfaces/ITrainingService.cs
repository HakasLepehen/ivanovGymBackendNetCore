using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface ITrainingService
{
    Task<List<TrainingDto>> GetTrainingsAsync();
    Task<TrainingDto> GetTrainingAsync(int id);
    Task<TrainingDto> CreateTrainingAsync(CreateTrainingDto model);
    Task DeleteTrainingAsync(int id);

    Task UpdateTrainingAsync(int id, TrainingDto model);
}
