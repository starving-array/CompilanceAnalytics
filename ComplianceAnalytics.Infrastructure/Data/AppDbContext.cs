using ComplianceAnalytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAnalytics.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table names
            modelBuilder.Entity<TaskEntity>().ToTable("Tasks");
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<LocationEntity>().ToTable("Locations");

            // Relationships
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Location)
                .WithMany(l => l.Tasks)
                .HasForeignKey(t => t.LocationID);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.CompletedUser)
                .WithMany(u => u.TasksCompleted)
                .HasForeignKey(t => t.CompletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
