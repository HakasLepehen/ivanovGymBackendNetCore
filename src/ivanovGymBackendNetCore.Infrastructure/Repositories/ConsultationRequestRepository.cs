using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

public class ConsultationRequestRepository : IConsultationRequestRepository
{
    
    private readonly AppDbContext _context;
    public ConsultationRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync(int id)
    {
        var consultationRequest = await _context.ConsultationRequests.FindAsync(id);

        if (consultationRequest == null)
            throw new Exception("Запрос на консультацию не найден");
        
        consultationRequest.IsCalled = true;
        await _context.SaveChangesAsync();
    }
    public async Task<ConsultationRequest> CreateRequestAsync(ConsultationRequest model)
    {
        await _context.ConsultationRequests.AddAsync(model);
        await _context.SaveChangesAsync();
        return model;
    }
    public async Task DeleteAsync(int id)
    {
        var consultationRequest = await _context.ConsultationRequests.FindAsync(id);

        if (consultationRequest == null)
            throw new Exception("Запрос на консультацию не найден");

        _context.ConsultationRequests.Remove(consultationRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ConsultationRequest>> GetAllAsync()
    {
        return await _context.ConsultationRequests.ToListAsync();
    }
}