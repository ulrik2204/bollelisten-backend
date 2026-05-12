#!/bin/bash
set -e

# Railway injects PORT; configure ASP.NET Core to listen on it
export ASPNETCORE_URLS="http://+:${PORT:-8080}"

# Build connection string from Railway's DATABASE_URL or individual PG* vars
if [ -n "$DATABASE_URL" ] && [ -z "$ConnectionStrings__bollelisten" ]; then
    # Parse DATABASE_URL (format: postgresql://user:password@host:port/dbname)
    proto="$(echo "$DATABASE_URL" | grep :// | sed -e's,^\(.*://\).*,\1,g')"
    url="${DATABASE_URL#$proto}"
    userpass="$(echo "$url" | cut -d@ -f1)"
    hostportdb="$(echo "$url" | cut -d@ -f2)"
    user="$(echo "$userpass" | cut -d: -f1)"
    pass="$(echo "$userpass" | cut -d: -f2)"
    hostport="$(echo "$hostportdb" | cut -d/ -f1)"
    host="$(echo "$hostport" | cut -d: -f1)"
    port="$(echo "$hostport" | cut -d: -f2)"
    db="$(echo "$hostportdb" | cut -d/ -f2 | cut -d'?' -f1)"
    export ConnectionStrings__bollelisten="Host=$host;Port=$port;Database=$db;Username=$user;Password=$pass;SSL Mode=Require;Trust Server Certificate=true"
    echo "Constructed connection string from DATABASE_URL"
elif [ -n "$PGHOST" ] && [ -z "$ConnectionStrings__bollelisten" ]; then
    export ConnectionStrings__bollelisten="Host=$PGHOST;Port=${PGPORT:-5432};Database=${PGDATABASE:-railway};Username=$PGUSER;Password=$PGPASSWORD;SSL Mode=Require;Trust Server Certificate=true"
    echo "Constructed connection string from PG* variables"
fi

echo "Running database migrations..."
dotnet /app/migrations/MigrationService.dll

echo "Starting API on port ${PORT:-8080}..."
exec dotnet /app/api/API.dll
