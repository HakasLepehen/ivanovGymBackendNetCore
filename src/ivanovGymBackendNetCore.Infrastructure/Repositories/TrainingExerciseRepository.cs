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

    public async Task<TrainingExercise> CreateExerciseAsync(TrainingExercise model)
    {
        await _context.TrainingExercises.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task<List<TrainingExercise>> GetAllAsync()
    {
        return await _context.TrainingExercises.ToListAsync();
    }

    public async Task<List<TrainingExercise>> GetAllByTrainingIdAsync(int id)
    {
        var trainings = await _context.TrainingExercises
            .Where(o => o.TrainingId == id)
            .ToListAsync();

        return trainings;
    }

    public async Task DeleteAsync(int id)
    {
        var targetExercise = await _context.TrainingExercises.FindAsync(id);

        if (targetExercise == null)
            throw new Exception("Указанное упражнение не найдено");

        _context.TrainingExercises.Remove(targetExercise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExerciseAsync(TrainingExercise model)
    {
        var targetModel = await _context.TrainingExercises.FindAsync(model.Id);

        if (targetModel == null)
            throw new Exception("Указанная тренировка не найдена");

        _context.TrainingExercises.Update(targetModel);
        await _context.SaveChangesAsync();
    }
}