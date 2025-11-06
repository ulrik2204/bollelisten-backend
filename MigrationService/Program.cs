
using Data;
using Microsoft.EntityFrameworkCore;
using MigrationService;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder();
builder.AddServiceDefaults();
builder.Services.AddHostedService<DbInitializer>();

builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

builder.AddNpgsqlDbContext<AppDbContext>("bollelisten", configureDbContextOptions: options =>
{
    options.UseNpgsql(npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly(typeof(DbInitializer).Assembly.FullName);
    });
});


var host = builder.Build();
host.Run();
