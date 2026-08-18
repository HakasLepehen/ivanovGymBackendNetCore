#!/bin/sh
set -e

# Read password from Docker secret if it exists
DB_PASSWORD=""
if [ -f "/run/secrets/postgres_password" ]; then
    DB_PASSWORD=$(cat /run/secrets/postgres_password | tr -d '[:space:]')
fi

# Build connection string with password
if [ -n "$DB_PASSWORD" ]; then
    export ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Database=myapp;Username=myuser;Password=$DB_PASSWORD"
else
    echo "WARNING: No database password found. Using appsettings.json connection string."
fi

echo "Applying migrations..."
dotnet ef database update \
  --project /app/ivanovGymBackendNetCore.Infrastructure \
  --startup-project /app/ivanovGymBackendNetCore.API

echo "Migrations applied. Starting API..."
exec dotnet /app/ivanovGymBackendNetCore.API.dll
