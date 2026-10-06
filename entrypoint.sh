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

# JWT signing key: Docker secret has priority over the environment variable.
if [ -f "/run/secrets/jwt_key" ]; then
    export JwtSettings__Key=$(cat /run/secrets/jwt_key | tr -d '[:space:]')
fi

if [ -z "$JwtSettings__Key" ]; then
    echo "ERROR: No JWT signing key found. Mount it as /run/secrets/jwt_key or pass JwtSettings__Key."
    exit 1
fi

echo "Applying migrations..."
/app/efbundle --connection "$ConnectionStrings__DefaultConnection"

echo "Migrations applied. Starting API..."
exec dotnet /app/ivanovGymBackendNetCore.API.dll
