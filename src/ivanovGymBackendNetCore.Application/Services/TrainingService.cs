using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services;

public class TrainingService : ITrainingService
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IMapper _mapper;

    public TrainingService(ITrainingRepository trainingRepository, IMapper mapper)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
    }

    public async Task<List<TrainingDto>> GetTrainingsAsync()
    {
        var trainings = await _trainingRepository.GetAllAsync();
        var trainingDtos = _mapper.Map<List<TrainingDto>>(trainings);
        return trainingDtos;
    }
}
