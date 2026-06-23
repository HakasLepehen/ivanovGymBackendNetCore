using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface ITrainingService
{
    Task<List<TrainingDto>> GetTrainingsAsync();
    Task<TrainingDto> CreateTrainingAsync(CreateTrainingDto model);
    Task DeleteTrainingAsync(int id);
}
