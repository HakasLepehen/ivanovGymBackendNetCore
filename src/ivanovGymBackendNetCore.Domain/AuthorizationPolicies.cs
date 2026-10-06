namespace ivanovGymBackendNetCore.Domain;

/// <summary>
/// Имена политик авторизации, объявленных в Program.cs.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>Только администратор (суперпользователь).</summary>
    public const string AdminOnly = "AdminOnly";

    /// <summary>Администратор и тренер.</summary>
    public const string StaffOnly = "StaffOnly";
}
