using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;

namespace ivanovGymBackendNetCore.Application.Services;

class TrainingExerciseService : ITrainingExerciseService
{
    private readonly ITrainingExerciseRepository _trainingExerciseRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    public TrainingExerciseService(ITrainingExerciseRepository trainingExerciseRepository, IMapper mapper, AppDbContext context)
    {
        _trainingExerciseRepository = trainingExerciseRepository;
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Получение списка относящихся к тренировке упражнений
    /// </summary>
    /// <param name="id">Идентификатор тренировки</param>
    /// <returns>Спискок относящихся к тренировке упражнений</returns>
    public async Task<List<TrainingExerciseDto>> GetTrainingsByTrainingIdAsync(int id)
    {
        List<TrainingExerciseDto> dtos = new List<TrainingExerciseDto>();

        var tExercises = await _trainingExerciseRepository.GetAllByTrainingIdAsync(id);
        if (tExercises != null)
        {
            dtos = _mapper.Map<List<TrainingExerciseDto>>(tExercises);
        }

        return dtos;
    }

    /// <summary>
    /// Создать упражнение в тренировке
    /// </summary>
    /// <param name="dto">Модель упражнения как часть тренировки</param>
    /// <returns>Сохраненное дто упражнения</returns>
    public async Task<TrainingExerciseDto> CreateTrainingExerciseAsync(TrainingExerciseDto dto)
    {
        TrainingExercise model = _mapper.Map<TrainingExercise>(dto);
        TrainingExercise savedModel = await _trainingExerciseRepository.CreateExerciseAsync(model);
        return _mapper.Map<TrainingExerciseDto>(savedModel);
    }

    public async Task DeleteTrainingExerciseAsync(int id)
    {
        await _trainingExerciseRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Редактирование упражнения
    /// </summary>
    /// <param name="dto">ДТО упражнения</param>
    /// <returns>Обновленное дто упражнения</returns>
    public async Task UpdateTrainingExerciseAsync(TrainingExerciseDto dto)
    {
        TrainingExercise model = _mapper.Map<TrainingExercise>(dto);
        await _trainingExerciseRepository.UpdateExerciseAsync(model);
    }

    /// <summary>
    /// Найти последнее исполнения конкретного упражнения по клиенту
    /// </summary>
    /// <param name="id">Идентификатор упражнения</param>
    /// <param name="dto">параметры упражнения</param>
    /// <returns></returns>
    public async Task<TrainingExerciseDto> FindLastExerciseAsync(int id, LastExerciseDto dto)
    {
        List<TrainingExercise> exercises = await _trainingExerciseRepository.FindLastExerciseAsync(id, dto?.Repetition, dto.Client);

        TrainingExercise? lastExecutionOfExercise = exercises
            .Where(e => e.Training is not null)
            .MaxBy(e => e.Training.PlannedDate);

        return _mapper.Map<TrainingExerciseDto>(lastExecutionOfExercise);
    }
}
