using ivanovGymBackendNetCore.Application.DTOs;

namespace ivanovGymBackendNetCore.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Регистрация учётной записи. Роль вычисляется на сервере: администратор — только по
    /// <see cref="Domain.Enums.UserRole.AdminEmail"/>, requestedRole принимает участие, только если
    /// её разрешил вызывающий (контроллер проверяет роль администратора).
    /// </summary>
    Task<string> SignUpAsync(string email, string password, string? requestedRole = null);

    Task<AuthResultDto> SignInAsync(string email, string password);

    /// <summary>
    /// Смена собственного пароля со знанием старого.
    /// </summary>
    Task ChangeOwnPasswordAsync(Guid userId, string oldPassword, string newPassword);

    /// <summary>
    /// Сброс пароля без старого пароля и без токена сброса.
    /// </summary>
    /// <param name="targetMustBeUser">
    /// true — сброс разрешён только учётной записи в роли «user» (вызов тренером).
    /// </param>
    Task ResetPasswordAsync(Guid targetUserId, string newPassword, bool targetMustBeUser);
}
