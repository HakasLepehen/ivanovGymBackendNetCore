using ivanovGymBackendNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}