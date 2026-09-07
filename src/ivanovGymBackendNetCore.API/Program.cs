using ivanovGymBackendNetCore.Application;
using ivanovGymBackendNetCore.Domain;
using ivanovGymBackendNetCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddSwaggerGen();

// Add application services (Application layer)
builder.Services.AddApplicationServices();

// Add infrastructure services (Infrastructure layer with PostgreSQL)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
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
                "http://localhost:5174"
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
