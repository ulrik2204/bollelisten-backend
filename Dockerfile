FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restore
COPY API/API.csproj API/
COPY Common/Common.csproj Common/
COPY Data/Data.csproj Data/
COPY ServiceDefaults/ServiceDefaults.csproj ServiceDefaults/
COPY MigrationService/MigrationService.csproj MigrationService/

RUN dotnet restore API/API.csproj && dotnet restore MigrationService/MigrationService.csproj

# Copy source and publish both projects
COPY . .
RUN dotnet publish API/API.csproj -c Release -o /app/api /p:UseAppHost=false
RUN dotnet publish MigrationService/MigrationService.csproj -c Release -o /app/migrations /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/api ./api
COPY --from=build /app/migrations ./migrations
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
