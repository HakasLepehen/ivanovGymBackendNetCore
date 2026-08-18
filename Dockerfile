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

# ---- Migrations stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS migrations
WORKDIR /src
COPY --from=build /src/src .
RUN dotnet tool install --global dotnet-ef

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Copy source and EF tools for migrations
COPY --from=migrations /root/.dotnet/tools /root/.dotnet/tools
COPY --from=migrations /src/src /app/src

# Copy entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PATH="/root/.dotnet/tools:${PATH}"

ENTRYPOINT ["/app/entrypoint.sh"]
