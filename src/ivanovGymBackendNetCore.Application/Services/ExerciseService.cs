using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IMapper _mapper;

    public ExerciseService(IExerciseRepository exerciseRepository, IMapper mapper)
    {
        _exerciseRepository = exerciseRepository;
        _mapper = mapper;
    }
    public async Task<List<ExerciseDto>> GetExercisesAsync()
    {
        List<Exercise> exercises = await _exerciseRepository.GetAllAsync();
        return _mapper.Map<List<ExerciseDto>>(exercises);
    }

    public async Task AddExerciseAsync(CreateExerciseDto dto)
    {
        Exercise exercise = _mapper.Map<Exercise>(dto);
        await _exerciseRepository.CreateAsync(exercise);
    }

    public async Task DeleteExerciseAsync(int id)
    {
        await _exerciseRepository.DeleteAsync(id);
    }
}
