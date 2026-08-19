# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so dependency restore remains cached when source changes.
COPY src/ivanovGymBackendNetCore.API/*.csproj ivanovGymBackendNetCore.API/
COPY src/ivanovGymBackendNetCore.Application/*.csproj ivanovGymBackendNetCore.Application/
COPY src/ivanovGymBackendNetCore.Domain/*.csproj ivanovGymBackendNetCore.Domain/
COPY src/ivanovGymBackendNetCore.Infrastructure/*.csproj ivanovGymBackendNetCore.Infrastructure/
RUN dotnet restore ivanovGymBackendNetCore.API/ivanovGymBackendNetCore.API.csproj

COPY src/ .
WORKDIR /src/ivanovGymBackendNetCore.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Собираем migration bundle — исполняемый файл, который применяет миграции
# без .NET SDK и без исходников. Использует runtime из aspnet-образа.
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations bundle \
  --project /src/ivanovGymBackendNetCore.Infrastructure \
  --startup-project /src/ivanovGymBackendNetCore.API \
  -o /app/efbundle

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Copy migration bundle
COPY --from=build /app/efbundle /app/efbundle
RUN chmod +x /app/efbundle

# Copy entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["/app/entrypoint.sh"]
