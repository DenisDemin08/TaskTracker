using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Storage
{
    public class TaskTrackerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tasks> Tasks { get; set; } 

        public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<Manager>().ToTable("Manager");
            modelBuilder.Entity<TeamMember>().ToTable("TeamMember");


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            });


            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(t => t.Name).IsUnique();

            entity.HasOne<Project>()
            .WithMany()
            .HasForeignKey(t => t.Project)
            .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Manager>()
                    .WithMany()
                    .HasForeignKey(t => t.ManagerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);

                entity.HasOne<Administrator>()
                    .WithMany()
                    .HasForeignKey(p => p.AdministratorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.TaskStatus)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(t => t.TaskPriority)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                entity.HasOne<Administrator>()
                    .WithMany()
                    .HasForeignKey(t => t.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<TeamMember>()
                    .WithMany()
                    .HasForeignKey(t => t.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Project>()
                    .WithMany()
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.Property(tm => tm.Position)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.HasOne<Team>()
                    .WithMany()
                    .HasForeignKey(tm => tm.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}