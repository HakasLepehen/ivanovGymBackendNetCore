using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ivanovGymBackendNetCore.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IClientService _clientService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<User> userManager,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger,
        IClientService clientService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
        _clientService = clientService;
    }

    public async Task<string> SignUpAsync(string email, string password)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            string errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        var client = new CreateClientDto
        {
            Email = email,
            Guid = user.Id,
            FullName = email,
            IsActive = true,
        };

        ClientDto savedClient = await _clientService.CreateClientAsync(client);

        user.ClientFkId = savedClient.Id;
        await _userManager.UpdateAsync(user);

        return await GenerateJwtTokenAsync(user);
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("Invalid credentials");
        bool passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            throw new Exception("Invalid credentials");
        }

        return await GenerateJwtTokenAsync(user);
    }

    public async Task<AuthResultDto> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");
        var client = await _clientService.GetClientByEmailAsync(email) ?? throw new Exception("Client not found");
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            throw new Exception("Неверный пароль");
        }

        if (!client.IsActive)
        {
            throw new Exception("Пользователь неактивен. Дальнейший вход невозможен");
        }

        var token = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        user.Token = refreshToken;
        await _userManager.UpdateAsync(user);

        return new AuthResultDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            Email = user.Email!,
            Roles = user.Roles
        };
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
