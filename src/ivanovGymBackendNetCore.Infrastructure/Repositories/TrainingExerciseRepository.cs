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
            throw new Exception("Указанное упражнение тренировки не найдено");

        // targetModel уже отслеживается контекстом, поэтому меняем свойства
        // прямо в нём: присваивание targetModel = model и Update(model)
        // привело бы к попытке отследить второй экземпляр с тем же ключом.
        targetModel.TrainingId = model.TrainingId;
        targetModel.ExerciseId = model.ExerciseId;
        targetModel.SetCount = model.SetCount;
        targetModel.ExecutionNumber = model.ExecutionNumber;
        targetModel.PayloadWeight = model.PayloadWeight;
        targetModel.Comment = model.Comment;

        await _context.SaveChangesAsync();
    }
    /// <summary>
    /// Поиск выполнений упражнения. Если executionNumber задан — только
    /// с таким же количеством повторений, иначе — все выполнения упражнения.
    /// </summary>
    public async Task<List<TrainingExercise>> FindLastExerciseAsync(int exerciseId, string? executionNumber, Guid client)
    {
        IQueryable<TrainingExercise> query = _context.TrainingExercises
            .AsNoTracking()
            .Where(e => e.Training.ClientGuid == client)
            .Include(o => o.Training)
            .Where(e => e.ExerciseId == exerciseId);

        if (!string.IsNullOrWhiteSpace(executionNumber))
        {
            query = query.Where(e => e.ExecutionNumber == executionNumber);
        }

        return await query.ToListAsync();
    }
}