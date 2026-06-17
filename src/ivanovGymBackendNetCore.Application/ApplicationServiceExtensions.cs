using ivanovGymBackendNetCore.Application.Interfaces;
using ivanovGymBackendNetCore.Application.Profiles;
using ivanovGymBackendNetCore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ivanovGymBackendNetCore.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<ITrainingService, TrainingService>();

        return services;
    }
}
