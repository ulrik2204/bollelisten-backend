using Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users =>  Set<User>();
    public DbSet<Entry> Entries =>  Set<Entry>();
    public DbSet<Group> Groups =>  Set<Group>();

    // public string DbPath { get; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(t => t.CreatedAt).HasDefaultValueSql("now()");

        modelBuilder.Entity<Group>().Property(t => t.CreatedAt).HasDefaultValueSql("now()");

        base.OnModelCreating(modelBuilder);

    }
}

