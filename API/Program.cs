using API.Configuration;
using Common.Extensions;
using Common.Services;
using ServiceDefaults;
using Scalar.AspNetCore;

// Step 1: Configure the builder
var builder = WebApplication.CreateBuilder(args);
// builder.Services.ConfigureOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();

builder.AddBollelistenDb();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5000", "https://bollelisten.rosby.no")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Added to support credentials
        });
});

builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IEntryService, EntryService>();


// Step 2: Build the application and setup endpoints
var app = builder.Build();

app.UseCors(myAllowSpecificOrigins);


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
else
{
    app.UseHttpsRedirection();
}
app.MapControllers();

// Step 3: Run the application
await app.RunAsync();
