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

    public Task CompleteAsync(int id) => throw new NotImplementedException();
    public Task CreateRequestAsync(ConsultationRequest model) => throw new NotImplementedException();
    public Task DeleteAsync(int id) => throw new NotImplementedException();
    public Task<List<ConsultationRequest>> GetAllAsync() => throw new NotImplementedException();
}