using ivanovGymBackendNetCore.Application;
using ivanovGymBackendNetCore.Domain;
using ivanovGymBackendNetCore.Domain.Enums;
using ivanovGymBackendNetCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string devPolicy = "allowedDevCORSConfig";
string prodPolicy = "allowedProdCORSConfig";
const string databasePasswordSecretPath = "/run/secrets/postgres_password";
if (File.Exists(databasePasswordSecretPath))
{
    // Compose передаёт файл только контейнеру, не меняя локальную конфигурацию.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");
    var connectionStringBuilder = new DbConnectionStringBuilder { ConnectionString = connectionString };
    connectionStringBuilder["Password"] = File.ReadAllText(databasePasswordSecretPath).Trim();
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionStringBuilder.ConnectionString;
}

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();

// Add application services (Application layer)
builder.Services.AddApplicationServices();

// Add infrastructure services (Infrastructure layer with PostgreSQL)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure JWT Authentication
// Ключ подписи берётся только из конфигурации (JwtSettings__Key либо /run/secrets/jwt_key,
// который монтирует entrypoint.sh). В репозитории ключа быть не должно.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

string? jwtKey = jwtSettings[nameof(JwtSettings.Key)];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "JwtSettings:Key отсутствует или короче 32 символов. Передайте ключ переменной окружения " +
        "JwtSettings__Key либо файлом /run/secrets/jwt_key. Для локальной разработки: " +
        "dotnet user-secrets --project src/ivanovGymBackendNetCore.API set \"JwtSettings:Key\"=<ключ-не-короче-32-символов>");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Ролевая модель. Источник ролей — users."Roles", они попадают в токен клеймом ClaimTypes.Role
// (см. AuthService.GenerateJwtTokenAsync), поэтому достаточно RequireRole.
builder.Services.AddAuthorization(options =>
{
    // Всё, что не помечено [AllowAnonymous], доступно только аутентифицированным.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Администратор — суперпользователь.
    options.AddPolicy(AuthorizationPolicies.AdminOnly, policy => policy.RequireRole(UserRole.Admin));

    // Администратор и тренер: работа с учётными записями пользователей.
    options.AddPolicy(AuthorizationPolicies.StaffOnly,
        policy => policy.RequireRole(UserRole.Admin, UserRole.Trainer));
});

// Configure CORS - allow requests from specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy(prodPolicy, policy => policy
            .WithOrigins(
                "https://app.omni-fit.ru",
                "https://omni-fit.ru",
                "https://www.omni-fit.ru",
                "http://app.omni-fit.ru",
                "http://omni-fit.ru",
                "http://www.omni-fit.ru"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );

    options.AddPolicy(devPolicy, policy => policy
            .WithOrigins(
                "https://localhost",
                "http://localhost",
                "https://localhost:5173",
                "http://localhost:5173",
                "https://localhost:5174",
                "http://localhost:5174",
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        );
});

// Configure Swagger with JWT Bearer authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Ivanov Gym API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Access token из POST /api/auth/login. Введите токен без префикса \"Bearer\"."
    });

    // В Microsoft.OpenApi 2.x требование ссылается на схему через OpenApiSecuritySchemeReference,
    // а Swashbuckle 10 передаёт документ в фабрике требования.
    c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc, null!),
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devPolicy);
}
else
{
    app.UseCors(prodPolicy);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
