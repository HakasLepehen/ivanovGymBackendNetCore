using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

class TrainingExerciseRepository : ITrainingExerciseRepository
{
    public readonly AppDbContext _context;

    public TrainingExerciseRepository(AppDbContext context)
    { 
        _context = context;
    }
    public async Task<List<TrainingExercise>> GetAllAsync()
    {
        return await _context.TrainingExercises.ToListAsync();
    }
}