using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace ivanovGymBackendNetCore.Application.Services;

public class TrainingService : ITrainingService
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public TrainingService(ITrainingRepository trainingRepository, IMapper mapper, AppDbContext context)
    {
        _trainingRepository = trainingRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<TrainingDto>> GetTrainingsAsync()
    {
        var trainings = await _trainingRepository.GetAllAsync();
        var trainingDtos = _mapper.Map<List<TrainingDto>>(trainings);
        return trainingDtos;
    }

    public async Task<TrainingDto> CreateTraining(CreateTrainingDto model)
    {
        Training dto = _mapper.Map<Training>(model);
        var training = await _trainingRepository.CreateAsync(dto);
        return _mapper.Map<TrainingDto>(training);
    }
}
