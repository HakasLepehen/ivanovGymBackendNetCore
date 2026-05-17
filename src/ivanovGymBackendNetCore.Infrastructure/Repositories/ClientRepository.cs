using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using ivanovGymBackendNetCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Client> CreateAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return client;
    }
    public async Task DeleteAsync(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }
    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.FindAsync(id);
    }
    public async Task<Client> UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        return client;
    }
    public async Task<Client?> GetByEmailAsync(string email)
    {
        return await _context.Clients.FirstAsync(c => c.Email == email);
    }
}
