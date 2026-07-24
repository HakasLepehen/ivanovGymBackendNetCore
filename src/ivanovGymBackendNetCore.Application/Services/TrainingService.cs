using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<TrainingDto> GetTrainingAsync(int id)
    {
        var training = await _trainingRepository.GetByIdAsync(id);
        if (training == null)
        {
            throw new Exception($"Тренировка с {id} не найдена");
        }
        var trainingDto = _mapper.Map<TrainingDto>(training);

        return trainingDto;
    }

    public async Task<TrainingDto> CreateTrainingAsync(CreateTrainingDto model)
    {
        Training dto = _mapper.Map<Training>(model);
        var training = await _trainingRepository.CreateAsync(dto);
        return _mapper.Map<TrainingDto>(training);
    }

    public async Task DeleteTrainingAsync(int id)
    {
        try
        {
            await _trainingRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Не удалось удалить тренировку с идентификатором {id}", ex);
        }
    }

    public async Task UpdateTrainingAsync(int id, TrainingDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            Training updatingTraining = _mapper.Map<Training>(dto);
            await _trainingRepository.UpdateAsync(id, updatingTraining);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Не удалось сохранить изменения в тренировке", ex);
        }
    }
}
