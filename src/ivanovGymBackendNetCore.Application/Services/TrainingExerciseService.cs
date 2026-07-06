using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
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
}
