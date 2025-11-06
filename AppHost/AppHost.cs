using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("admin-username");
var password = builder.AddParameter("admin-password", true);

var postgres = builder
    .AddPostgres("postgres", username, password, 55483)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgWeb(cfg => cfg.WithHostPort(55484));

var db = postgres.AddDatabase("bollelisten");

builder.AddProject<MigrationService>("Migrations").WaitFor(postgres).WithReference(db);
builder.AddProject<API>("API").WaitFor(db).WithReference(db);

builder.Build().Run();