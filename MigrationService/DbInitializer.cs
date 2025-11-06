using System.Diagnostics;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MigrationService;

public class DbInitializer: BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<DbInitializer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime, ILogger<DbInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await EnsureDatabaseAsync(context, stoppingToken);
            await RunMigrationAsync(context, stoppingToken);
            // await SeedDataAsync(context, stoppingToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception on migration");
            activity?.AddException(exception);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(AppDbContext dbAppDbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbAppDbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbAppDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
                await dbCreator.CreateAsync(cancellationToken);
        });
    }

    private static async Task RunMigrationAsync(AppDbContext dbAppDbContext, CancellationToken cancellationToken)
    {
        var strategy = dbAppDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbAppDbContext.Database.MigrateAsync(cancellationToken);
        });
    }

}