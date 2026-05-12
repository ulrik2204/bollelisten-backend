#!/bin/bash
set -e

# Railway injects PORT; configure ASP.NET Core to listen on it
export ASPNETCORE_URLS="http://+:${PORT:-8080}"

echo "Running database migrations..."
dotnet /app/migrations/MigrationService.dll

echo "Starting API on port ${PORT:-8080}..."
exec dotnet /app/api/API.dll
