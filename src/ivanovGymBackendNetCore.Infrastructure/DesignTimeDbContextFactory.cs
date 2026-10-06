using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ivanovGymBackendNetCore.Infrastructure.Data;

namespace ivanovGymBackendNetCore.Infrastructure;

/// <summary>
/// Точка входа для инструментов EF: <c>dotnet ef migrations add|bundle|script</c> и т. п.
/// Без неё EF пытается «запустить» стартовый проект (ivanovGymBackendNetCore.API), не находит
/// в нём <see cref="IDesignTimeDbContextFactory{TContext}"/> и падает с
/// «Unable to create a 'DbContext' of type ''». В Docker-образе стартового проекта вообще нет,
/// поэтому фабрика обязательна — именно она позволяет собирать efbundle.dll при сборке образа.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string ConnectionStringKey = "ConnectionStrings:DefaultConnection";

    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        // EF_CONNECTIONSTRING позволяет собрать бандл без appsettings.json вообще.
        var connectionString =
            Environment.GetEnvironmentVariable("EF_CONNECTIONSTRING")
            ?? configuration[ConnectionStringKey];

        if (connectionString is null)
        {
            Console.Error.WriteLine(
                "Внимание: строка подключения \"{0}\" не найдена. " +
                "Используется дефолтное значение — для создания миграций это нормально, " +
                "но для database update нужно указать реальную строку.",
                ConnectionStringKey);

            connectionString = "Host=localhost;Port=5432;Database=ivangymdb;Username=postgres;Password=postgres";
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder();

        var settingsPath = FindAppSettingsPath();
        if (settingsPath is not null)
        {
            builder.SetBasePath(Path.GetDirectoryName(settingsPath)!);
            builder.AddJsonFile(Path.GetFileName(settingsPath), optional: true, reloadOnChange: false);
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);
        }

        // Переменные окружения перекрывают appsettings — как в рантайме приложения.
        builder.AddEnvironmentVariables();

        return builder.Build();
    }

    /// <summary>
    /// Ищет appsettings.json стартового проекта: рабочий каталог EF указывает то на корень
    /// решения, то на каталог проекта, поэтому поднимаемся вверх по дереву каталогов
    /// от текущей директории и от расположения сборки.
    /// </summary>
    private static string? FindAppSettingsPath()
    {
        var starts = new[] { Environment.CurrentDirectory, AppContext.BaseDirectory };

        foreach (var start in starts)
        {
            var directory = new DirectoryInfo(start);

            while (directory is not null)
            {
                var found = FindInLevel(directory);
                if (found is not null)
                {
                    return found;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }

    private static string? FindInLevel(DirectoryInfo directory)
    {
        // На уровне решения файл лежит в подкаталоге стартового проекта (src/API).
        var candidates = Directory.EnumerateFiles(directory.FullName, "appsettings.json", SearchOption.TopDirectoryOnly)
            .Concat(Directory.EnumerateFiles(directory.FullName, "appsettings.json", SearchOption.AllDirectories)
                .Where(static path => !IsBuildOutput(path)));

        return candidates.FirstOrDefault();
    }

    private static bool IsBuildOutput(string path)
    {
        var segments = path.Split(Path.DirectorySeparatorChar);
        return segments.Contains("bin") || segments.Contains("obj") || segments.Contains(".git");
    }
}
