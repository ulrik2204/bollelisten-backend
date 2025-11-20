using API.Middleware;
using Common.Extensions;
using Common.Services;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

// Step 1: Configure the builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();

builder.AddBollelistenDb();

builder.Services.AddMemoryCache();

builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IGroupService, GroupService>();



// Step 2: Build the application and setup endpoints
var app = builder.Build();


app.UseMiddleware<CookieAuthMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapControllers();

// Step 3: Run the application
await app.RunAsync();
