using Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> People =>  Set<Person>();
    public DbSet<Entry> Entries =>  Set<Entry>();
    public DbSet<Group> Groups =>  Set<Group>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().Property(t => t.CreatedAt).HasDefaultValueSql("now()");

        modelBuilder.Entity<Group>().Property(t => t.CreatedAt).HasDefaultValueSql("now()");

        base.OnModelCreating(modelBuilder);

    }
}

