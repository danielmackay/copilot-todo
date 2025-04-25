using Api.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Common;

public class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos => Set<Todo>();

    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
        });
    }
}