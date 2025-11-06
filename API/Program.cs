using Common.Extensions;
using Data;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

// Step 1: Configure the builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();


builder.AddBollelistenDb();


// Step 2: Build the application and setup endpoints
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapControllers();

// Step 3: Run the application
await app.RunAsync();
