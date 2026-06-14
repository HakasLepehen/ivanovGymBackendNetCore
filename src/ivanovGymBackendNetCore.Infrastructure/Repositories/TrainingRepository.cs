using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

public class TrainingRepository : ITrainingRepository
{
    private readonly AppDbContext _context;
    public TrainingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteTraining(int id)
    {
        var targetTraining = await _context.Trainings.FindAsync(id);

        if (targetTraining == null)
            throw new Exception("Указанная тренировка не найдена");

        targetTraining.IsCompleted = true;
        _context.Trainings.Update(targetTraining);
        await _context.SaveChangesAsync();
    }
    public Task<Training> CreateAsync(Training model) => throw new NotImplementedException();
    public async Task DeleteAsync(int id)
    {
        var targetTraining = await _context.Trainings.FindAsync(id);

        if (targetTraining == null)
            throw new Exception("Указанная тренировка не найдена");

        _context.Trainings.Remove(targetTraining);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Training>> GetAllAsync()
    {
        var trainings = await _context.Trainings.ToListAsync();
        return trainings;
    }
    public async Task<Training?> GetByIdAsync(int id)
    {
        var targetTraining = await _context.Trainings.FindAsync(id);

        return targetTraining;
    }
    public async Task UpdateAsync(Training model)
    {
        var targetTraining = await _context.Trainings.FindAsync(model.Id);

        if (targetTraining == null)
            throw new Exception("Указанная тренировка не найдена");

        _context.Trainings.Update(targetTraining);
        await _context.SaveChangesAsync();
    }
}
