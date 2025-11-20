using API.Configuration;
using API.Middleware;
using API.Services;
using Common.Extensions;
using Common.Services;
using ServiceDefaults;
using Scalar.AspNetCore;

// Step 1: Configure the builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureOpenApi();
builder.Services.AddControllers();
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();

builder.AddBollelistenDb();

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<ISoftAuthService, SoftAuthService>();


// Step 2: Build the application and setup endpoints
var app = builder.Build();


app.UseMiddleware<CookieAuthMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.MapGet("/", (context) =>
    {
        context.Response.Redirect("/scalar/v1");
        return Task.CompletedTask;
    });
    app.MapGet("/swagger", (context) =>
    {
        context.Response.Redirect("/scalar/v1");
        return Task.CompletedTask;
    });
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        // https://github.com/dotnet/aspnetcore/issues/57332#issuecomment-2479286855
        options.Servers = [];
    });
}

app.UseHttpsRedirection();
app.MapControllers();

// Step 3: Run the application
await app.RunAsync();