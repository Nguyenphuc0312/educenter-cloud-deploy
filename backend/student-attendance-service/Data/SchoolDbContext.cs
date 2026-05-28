using Microsoft.EntityFrameworkCore;
using StudentAttendanceService.Models;

namespace StudentAttendanceService.Data;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(20);
            entity.Property(x => x.Address).HasMaxLength(250);
            entity.Property(x => x.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(x => x.Status).HasMaxLength(50);
            entity.HasOne(x => x.Student)
                  .WithMany(x => x.Enrollments)
                  .HasForeignKey(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.Property(x => x.Note).HasMaxLength(250);
            entity.HasOne(x => x.Student)
                  .WithMany(x => x.Attendances)
                  .HasForeignKey(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.Property(x => x.Score).HasPrecision(5, 2);
            entity.Property(x => x.Note).HasMaxLength(250);
            entity.HasOne(x => x.Student)
                  .WithMany(x => x.Results)
                  .HasForeignKey(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.StudentId);
            entity.HasIndex(x => x.CourseId);
        });

    }
}
