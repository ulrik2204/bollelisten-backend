using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Common.Extensions;

public static class DatabaseContextExtensions
{
    public static WebApplicationBuilder AddBollelistenDb(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AppDbContext>("bollelisten");
        return builder;
    }
}