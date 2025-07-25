using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;
using Task = TaskManagement.API.Models.Task;

namespace TaskManagement.API.Data;

public class TaskManagementContext : DbContext
{
    public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options)
    {
    }

    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.AssignedTo)
            .WithMany(tm => tm.AssignedTasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}