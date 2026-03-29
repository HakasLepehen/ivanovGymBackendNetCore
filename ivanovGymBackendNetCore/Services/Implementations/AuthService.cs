using ivanovGymBackendNetCore.Data;
using ivanovGymBackendNetCore.DTO.Auth;
using ivanovGymBackendNetCore.Models;
using ivanovGymBackendNetCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ivanovGymBackendNetCore.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    // private readonly ITokenService _tokenService;
    // private readonly IPasswordHasher _passwordHasher;
    
    public AuthService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<UserDTO> SignUpAsync(SignUpRequestDTO model)
    {
        if (model?.Email is null)
        {
            throw new ArgumentException("Email обязателен для регистрации пользователя", nameof(model));
        }
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        User user = new User()
        {
            Email = model.Email,
            Password = model.Password,
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDTO()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin =  user.IsAdmin
        };
    }
}