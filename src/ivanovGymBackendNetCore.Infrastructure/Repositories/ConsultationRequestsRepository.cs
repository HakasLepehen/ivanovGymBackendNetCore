using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

public class ConsultationRequestsRepository : IConsultationRequestsRepository
{

    private readonly AppDbContext _context;
    public ConsultationRequestsRepository(AppDbContext context)
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
    public async Task CreateRequestAsync(ConsultationRequest model)
    {
        await _context.ConsultationRequests.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    public async Task CompleteRequestAsync(int id)
    {
        ConsultationRequest request = await _context.ConsultationRequests.FindAsync(id);

        if (request == null)
            throw new Exception("Запрос на консультацию не найден");

        request.IsCalled = true;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Очистить таблицу запросов с сайта не помеченных флагом обзвона.
    /// </summary>
    public async Task<List<ConsultationRequest>> RemoveUnansweredRequestsFromDB(bool resetIdentity = true, CancellationToken cancellationToken = default)
    {
        await _context.ConsultationRequests
                .Where(r => r.IsCalled)
                .ExecuteDeleteAsync(cancellationToken);

        return await GetAllAsync();
    }

    public async Task<List<ConsultationRequest>> GetAllAsync()
    {
        return await _context.ConsultationRequests.ToListAsync();
    }
}