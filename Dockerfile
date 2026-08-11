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

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Runtime-образ .NET предоставляет этого непривилегированного пользователя.
USER app
ENTRYPOINT ["dotnet", "ivanovGymBackendNetCore.API.dll"]
