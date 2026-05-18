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
        Console.WriteLine($"[DEBUG] Поиск клиента по email: '{email}'");
        Console.WriteLine($"[DEBUG] Длина email: {email.Length}");

        // Проверка на пробелы
        if (email != email.Trim())
        {
            Console.WriteLine($"[DEBUG] Email содержит пробелы! Trimmed: '{email.Trim()}'");
        }

        var clients = await _context.Clients.ToListAsync();
        Console.WriteLine($"[DEBUG] Всего клиентов в БД: {clients.Count}");

        foreach (var c in clients.Take(5))
        {
            Console.WriteLine($"[DEBUG]   - ID: {c.Id}, Email: '{c.Email}' (len={c.Email.Length})");
        }

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.Trim().ToLower());

        if (client == null)
        {
            Console.WriteLine("[DEBUG] Не удалось найти клиента");
            return null;
        }

        Console.WriteLine($"[DEBUG] Найдён клиент: ID={client.Id}, Email={client.Email}");
        return client;
    }
}
