using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Storage
{
    public class TaskTrackerdbContext(DbContextOptions<TaskTrackerdbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Managers> Managers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Teams> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.ToTable("Administrators");
                entity.HasKey(a => a.AdminId);

                entity.HasOne(a => a.User)
                    .WithOne()
                    .HasForeignKey<Administrators>(a => a.AdminId)
                    .HasPrincipalKey<Users>(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Managers>(entity =>
            {
                entity.ToTable("Managers");
                entity.HasKey(m => m.ManagerId);

                entity.HasOne(m => m.User)
                    .WithOne()
                    .HasForeignKey<Managers>(m => m.ManagerId)
                    .HasPrincipalKey<Users>(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.Position)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Employees>(e => e.EmployeeId)
                    .HasPrincipalKey<Users>(u => u.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.ToTable("Projects");
                entity.HasKey(p => p.ProjectId);
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("Teams");
                entity.HasKey(t => t.TeamId);
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(t => t.TaskId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}