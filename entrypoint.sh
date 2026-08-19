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
    echo "ERROR: No database password found. Create .secrets/postgres_password or pass a password secret, and restart with 'docker compose up -d'."
    exit 1
fi

echo "Applying migrations..."
/app/efbundle --connection "$ConnectionStrings__DefaultConnection"

echo "Migrations applied. Starting API..."
exec dotnet /app/ivanovGymBackendNetCore.API.dll
