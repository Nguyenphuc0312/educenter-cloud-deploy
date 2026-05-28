using System;
using CourseAndScheduleService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAndScheduleService.Data
{
    public class EduCenterDbContext : DbContext
    {
        public EduCenterDbContext(DbContextOptions<EduCenterDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entity properties to actual DB column names to avoid Invalid column name errors
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.Property(e => e.CourseID).HasColumnName("Id");
                entity.Property(e => e.CourseName).HasColumnName("CourseName");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.Credits).HasColumnName("Credits");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedAt");
                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedAt");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Classes");
                entity.Property(e => e.ClassID).HasColumnName("Id");
                entity.Property(e => e.ClassCode).HasColumnName("ClassCode");
                entity.Property(e => e.CourseID).HasColumnName("CourseId");
                entity.Property(e => e.Capacity).HasColumnName("Capacity");
                entity.Property(e => e.EnrolledStudents).HasColumnName("EnrolledStudents");
                entity.Property(e => e.Instructor).HasColumnName("Instructor");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedAt");
                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedAt");

                // Relationship configuration
                entity.HasOne(c => c.Course)
                      .WithMany(c => c.Classes)
                      .HasForeignKey(c => c.CourseID)
                      .HasConstraintName("FK_Classes_Courses_CourseId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedules");
                entity.Property(e => e.ScheduleID).HasColumnName("Id");
                entity.Property(e => e.ClassID).HasColumnName("ClassId");
                entity.Property(e => e.Room).HasColumnName("Room");
                entity.Property(e => e.ClassDate).HasColumnName("ClassDate");
                entity.Property(e => e.StartTime).HasColumnName("StartTime");
                entity.Property(e => e.EndTime).HasColumnName("EndTime");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");

                // Relationship configuration
                entity.HasOne(s => s.Class)
                      .WithMany(c => c.Schedules)
                      .HasForeignKey(s => s.ClassID)
                      .HasConstraintName("FK_Schedules_Classes_ClassId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // SEED DỮ LIỆU MẪU (ĐÃ NÂNG CẤP)
            // ==========================================
            var fixedDate = new DateTime(2024, 5, 26, 8, 0, 0); // Sử dụng ngày giờ tĩnh

            // 1. Thêm 5 Khóa học
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = 1, CourseName = "C# Basics", Description = "Khóa học cơ bản về C# và .NET", Credits = 3, Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Course { CourseID = 2, CourseName = "Advanced ASP.NET Core", Description = "Khóa học nâng cao về ASP.NET Core Web API", Credits = 4, Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Course { CourseID = 3, CourseName = "Flutter Mobile Dev", Description = "Phát triển ứng dụng di động đa nền tảng với Flutter", Credits = 3, Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Course { CourseID = 4, CourseName = "AI & Machine Learning", Description = "Ứng dụng AI/ML trong phân tích dữ liệu và dự đoán", Credits = 4, Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Course { CourseID = 5, CourseName = "Odoo ERP Framework", Description = "Quản trị doanh nghiệp và lập trình module với Odoo", Credits = 3, Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate }
            );

            // 2. Thêm 6 Lớp học (Phân bổ vào các khóa học)
            modelBuilder.Entity<Class>().HasData(
                new Class { ClassID = 1, ClassCode = "CS001", CourseID = 1, Capacity = 30, EnrolledStudents = 25, Instructor = "Nguyễn Văn A", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Class { ClassID = 2, ClassCode = "CS002", CourseID = 1, Capacity = 35, EnrolledStudents = 28, Instructor = "Trần Thị B", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Class { ClassID = 3, ClassCode = "ASP001", CourseID = 2, Capacity = 25, EnrolledStudents = 20, Instructor = "Lê Văn C", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Class { ClassID = 4, ClassCode = "FLT001", CourseID = 3, Capacity = 40, EnrolledStudents = 38, Instructor = "Phạm Đình D", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Class { ClassID = 5, ClassCode = "AIML01", CourseID = 4, Capacity = 20, EnrolledStudents = 15, Instructor = "Hoàng Anh E", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate },
                new Class { ClassID = 6, ClassCode = "ODOO01", CourseID = 5, Capacity = 30, EnrolledStudents = 10, Instructor = "Vũ Thị F", Status = "Active", CreatedDate = fixedDate, UpdatedDate = fixedDate }
            );

            // 3. Thêm 8 Lịch học chi tiết
            modelBuilder.Entity<Schedule>().HasData(
                new Schedule { ScheduleID = 1, ClassID = 1, Room = "A101", ClassDate = new DateTime(2026, 6, 1), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 2, ClassID = 1, Room = "A101", ClassDate = new DateTime(2026, 6, 3), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 3, ClassID = 2, Room = "B202", ClassDate = new DateTime(2026, 6, 1), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 4, ClassID = 3, Room = "C303", ClassDate = new DateTime(2026, 6, 2), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(11, 30, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 5, ClassID = 4, Room = "Lab-1", ClassDate = new DateTime(2026, 6, 4), StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 6, ClassID = 4, Room = "Lab-1", ClassDate = new DateTime(2026, 6, 6), StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 7, ClassID = 5, Room = "Lab-AI", ClassDate = new DateTime(2026, 6, 5), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate },
                new Schedule { ScheduleID = 8, ClassID = 6, Room = "D404", ClassDate = new DateTime(2026, 6, 5), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(17, 0, 0), Status = "Active", CreatedAt = fixedDate, UpdatedAt = fixedDate }
            );
        }
    }
}