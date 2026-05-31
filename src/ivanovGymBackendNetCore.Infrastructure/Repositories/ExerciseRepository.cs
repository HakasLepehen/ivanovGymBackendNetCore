using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _context;

    public ExerciseRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Exercise> CreateAsync(Exercise model)
    {
        await _context.Exercises.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }
    public async Task DeleteAsync(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id)
            ?? throw new Exception("Упражнение с идентификатором {{id}}");

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Exercise>> GetAllAsync()
    {
        return await _context.Exercises.ToListAsync();
    }
    public async Task<Exercise?> GetByIdAsync(int Id)
    {
        return await _context.Exercises.FindAsync(Id);
    }
    public async Task<Exercise> UpdateAsync(Exercise model)
    {
        _context.Exercises.Update(model);
        await _context.SaveChangesAsync();
        return model;
    }
}
