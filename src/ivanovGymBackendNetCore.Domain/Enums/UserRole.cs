namespace ivanovGymBackendNetCore.Domain.Enums;

/// <summary>
/// Имена ролей. Сравниваются с клеймом ClaimTypes.Role в JWT регистрозависимо,
/// поэтому канон — нижний регистр (совпадает с DEFAULT ARRAY['user']::text[] в БД).
/// Источник ролей — колонка users."Roles"; таблицы Identity (AspNetRoles/AspNetUserRoles) не используются.
/// </summary>
public static class UserRole
{
    public const string Admin = "admin";
    public const string Trainer = "trainer";
    public const string User = "user";

    /// <summary>
    /// Роли, которые администратор может выдать при регистрации новой учётной записи.
    /// Роль администратора здесь отсутствует: она выдаётся только по <see cref="AdminEmail"/>.
    /// </summary>
    public static readonly string[] AssignableByAdmin = { Trainer, User };

    /// <summary>
    /// Email единственной учётной записи администратора. Роль администратора выдаётся только
    /// при совпадении email из регистрации с этим значением. СОВПАДЕНИЕ РЕГИСТР НЕ УЧИТЫВАЕТСЯ.
    /// </summary>
    public const string AdminEmail = "REPLACE_WITH_ADMIN_EMAIL@example.com";

    /// <summary>
    /// Разрешена ли роль к выдаче администратором при регистрации другой учётной записи.
    /// </summary>
    public static bool IsAssignableByAdmin(string? role)
        => !string.IsNullOrWhiteSpace(role)
           && AssignableByAdmin.Contains(role.Trim().ToLowerInvariant());

    public static bool IsAdminEmail(string? email)
        => !string.IsNullOrWhiteSpace(email)
           && string.Equals(email.Trim(), AdminEmail, StringComparison.OrdinalIgnoreCase);
}
