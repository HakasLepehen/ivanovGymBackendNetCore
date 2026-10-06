using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ivanovGymBackendNetCore.Application.DTOs;
using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Domain;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Enums;
using ivanovGymBackendNetCore.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public async Task<string> SignUpAsync(string email, string password, string? requestedRole = null)
    {
        string normalizedEmail = email.Trim();
        string role = await ResolveRoleAsync(normalizedEmail, requestedRole);

        var user = new User
        {
            UserName = normalizedEmail,
            Email = normalizedEmail,
            EmailConfirmed = true,
            // Роль задаётся до CreateAsync, чтобы роль была известна уже в этом INSERT
            // и не требовалась вторая запись в users."Roles".
            Roles = new[] { role }
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            string errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        var client = new CreateClientDto
        {
            Email = normalizedEmail,
            Guid = user.Id,
            FullName = normalizedEmail,
            IsActive = true,
        };

        ClientDto savedClient = await _clientService.CreateClientAsync(client);

        user.ClientFkId = savedClient.Id;
        await _userManager.UpdateAsync(user);

        return await GenerateJwtTokenAsync(user);
    }

    public async Task<AuthResultDto> SignInAsync(string email, string password)
    {
        string normalizedEmail = email.Trim();
        var user = await _userManager.FindByEmailAsync(normalizedEmail) ?? throw new Exception("Пользователь не найден");
        var client = await _clientService.GetClientByEmailAsync(normalizedEmail) ?? throw new Exception("Клиент не найден");
        bool passwordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            throw new Exception("Неверный пароль");
        }

        if (!client.IsActive)
        {
            throw new Exception("Пользователь неактивен. Дальнейший вход невозможен");
        }

        string token = await GenerateJwtTokenAsync(user);
        string refreshToken = GenerateRefreshToken();

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

    public async Task ChangeOwnPasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("Пользователь не найден");

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        if (!result.Succeeded)
        {
            string errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Не удалось сменить пароль: {errors}");
        }

        _logger.LogInformation("Пользователь {UserId} сменил пароль", userId);
    }

    public async Task ResetPasswordAsync(Guid targetUserId, string newPassword, bool targetMustBeUser)
    {
        var user = await _userManager.FindByIdAsync(targetUserId.ToString())
            ?? throw new Exception("Пользователь не найден");

        if (targetMustBeUser && !user.Roles.Contains(UserRole.User))
        {
            throw new Exception("Тренер может сбрасывать пароль только учётной записи пользователя");
        }

        // Сброс выполняется без старого пароля, поэтому новый пароль проверяется до удаления
        // текущего: иначе при отказе валидации пользователь остался бы без пароля вовсе.
        await ValidateNewPasswordAsync(user, newPassword);

        var removed = await _userManager.RemovePasswordAsync(user);
        if (!removed.Succeeded)
        {
            string errors = string.Join("; ", removed.Errors.Select(e => e.Description));
            throw new Exception($"Не удалось сбросить пароль: {errors}");
        }

        var added = await _userManager.AddPasswordAsync(user, newPassword);
        if (!added.Succeeded)
        {
            string errors = string.Join("; ", added.Errors.Select(e => e.Description));
            throw new Exception($"Старый пароль удалён, а новый установлен не был: {errors}");
        }

        _logger.LogInformation("Пользователю {UserId} сброшен пароль", targetUserId);
    }

    /// <summary>
    /// Вычисление роли для новой учётной записи. Роль никогда не берётся из запроса напрямую.
    /// </summary>
    private async Task<string> ResolveRoleAsync(string email, string? requestedRole)
    {
        if (UserRole.IsAdminEmail(email))
        {
            // Email администратора открыт в исходном коде, а регистрация доступна из интернета,
            // поэтому защита от повторной выдачи роли обязательна.
            bool adminExists = await _userManager.Users.AnyAsync(u => u.Roles.Contains(UserRole.Admin));
            if (adminExists)
            {
                throw new Exception("Учётная запись администратора уже зарегистрирована");
            }

            return UserRole.Admin;
        }

        if (string.IsNullOrWhiteSpace(requestedRole))
        {
            return UserRole.User;
        }

        if (!UserRole.IsAssignableByAdmin(requestedRole))
        {
            throw new Exception($"Роль \"{requestedRole}\" нельзя назначить при регистрации");
        }

        return requestedRole.Trim().ToLowerInvariant();
    }

    private async Task ValidateNewPasswordAsync(User user, string newPassword)
    {
        foreach (var validator in _userManager.PasswordValidators)
        {
            var result = await validator.ValidateAsync(_userManager, user, newPassword);
            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Новый пароль не принят: {errors}");
            }
        }
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

        foreach (string role in user.Roles)
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
        byte[] randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
