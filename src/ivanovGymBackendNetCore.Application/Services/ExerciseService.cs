using AutoMapper;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;

namespace ivanovGymBackendNetCore.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciserepository;
    private readonly IMapper _mapper;

    public ExerciseService(IExerciseRepository exerciserepository, IMapper mapper)
    {
        _exerciserepository = exerciserepository;
        _mapper = mapper;
    }
    public async Task<List<ExerciseDto>> GetExercisesAsync()
    {
        List<Exercise> exercises = await _exerciserepository.GetAllAsync();
        return _mapper.Map<List<ExerciseDto>>(exercises);
    }
}
