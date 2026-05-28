using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Debt> Debts => Set<Debt>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudentName).HasMaxLength(100);
            entity.Property(e => e.CourseName).HasMaxLength(200);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionCode).HasMaxLength(50);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
        });

        // Debt
        modelBuilder.Entity<Debt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudentName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.CourseName).HasMaxLength(200);
            entity.Property(e => e.TotalFee).HasPrecision(18, 2);
            entity.Property(e => e.RemainingAmount).HasPrecision(18, 2);
        });

        // Payroll
        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TeacherName).HasMaxLength(100);
            entity.Property(e => e.Specialty).HasMaxLength(100);
            entity.Property(e => e.BaseSalary).HasPrecision(18, 2);
            entity.Property(e => e.Bonus).HasPrecision(18, 2);
            entity.Property(e => e.Deduction).HasPrecision(18, 2);
            entity.Property(e => e.NetSalary).HasPrecision(18, 2);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "Nguyễn Văn An", Email = "admin@educenter.vn", Password = "admin123", Role = "admin", Phone = "0900000001", Status = "Active", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, FullName = "Trần Thị Bình", Email = "binh.tran@educenter.vn", Password = "teacher123", Role = "teacher", Phone = "0901234567", Status = "Active", CreatedAt = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 3, FullName = "Phạm Đức Cường", Email = "cuong.pham@educenter.vn", Password = "teacher123", Role = "teacher", Phone = "0912345678", Status = "Active", CreatedAt = new DateTime(2024, 8, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 4, FullName = "Lê Minh Châu", Email = "chau.le@educenter.vn", Password = "student123", Role = "student", Phone = "0901111111", Status = "Active", CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 5, FullName = "Nguyễn Hoàng Dũng", Email = "dung.nh@educenter.vn", Password = "student123", Role = "student", Phone = "0902222222", Status = "Active", CreatedAt = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 6, FullName = "Phạm Thị Hà", Email = "ha.pt@educenter.vn", Password = "student123", Role = "student", Phone = "0903333333", Status = "Active", CreatedAt = new DateTime(2026, 1, 14, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 7, FullName = "Trần Văn Khoa", Email = "khoa.tv@educenter.vn", Password = "student123", Role = "student", Phone = "0904444444", Status = "Active", CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 8, FullName = "Đỗ Thị Lan", Email = "lan.dt@educenter.vn", Password = "student123", Role = "student", Phone = "0905555555", Status = "Active", CreatedAt = new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment { Id = 1, StudentId = 4, CourseId = 1, StudentName = "Lê Minh Châu", CourseName = "Tiếng Anh Giao Tiếp CB", Amount = 3500000, PaidAmount = 3500000, Status = "Paid", PaymentMethod = "BankTransfer", TransactionCode = "TXN001", PaymentDate = new DateTime(2026, 1, 18, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 2, StudentId = 4, CourseId = 3, StudentName = "Lê Minh Châu", CourseName = "Lập Trình Python CB", Amount = 4000000, PaidAmount = 2000000, Status = "Partial", PaymentMethod = "Cash", PaymentDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 3, StudentId = 5, CourseId = 2, StudentName = "Nguyễn Hoàng Dũng", CourseName = "IELTS 6.5+", Amount = 8500000, PaidAmount = 0, Status = "Unpaid", DueDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 4, StudentId = 6, CourseId = 4, StudentName = "Phạm Thị Hà", CourseName = "Lập Trình Web Full-Stack", Amount = 12000000, PaidAmount = 12000000, Status = "Paid", PaymentMethod = "VNPay", TransactionCode = "TXN002", PaymentDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 5, StudentId = 7, CourseId = 5, StudentName = "Trần Văn Khoa", CourseName = "Thiết Kế Đồ Họa", Amount = 3000000, PaidAmount = 0, Status = "Unpaid", DueDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 6, StudentId = 8, CourseId = 1, StudentName = "Đỗ Thị Lan", CourseName = "Tiếng Anh Giao Tiếp CB", Amount = 3500000, PaidAmount = 1000000, Status = "Partial", PaymentMethod = "MoMo", PaymentDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 7, StudentId = 5, CourseId = 6, StudentName = "Nguyễn Hoàng Dũng", CourseName = "Digital Marketing", Amount = 6000000, PaidAmount = 6000000, Status = "Paid", PaymentMethod = "BankTransfer", TransactionCode = "TXN003", PaymentDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Payment { Id = 8, StudentId = 4, CourseId = 7, StudentName = "Lê Minh Châu", CourseName = "Kế Toán Doanh Nghiệp", Amount = 4500000, PaidAmount = 0, Status = "Unpaid", DueDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Debts
        modelBuilder.Entity<Debt>().HasData(
            new Debt { Id = 1, StudentId = 4, StudentName = "Lê Minh Châu", Email = "chau.le@educenter.vn", CourseId = 3, CourseName = "Lập Trình Python CB", TotalFee = 4000000, RemainingAmount = 2000000, DaysOverdue = 10, Status = "Warning", DueDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Debt { Id = 2, StudentId = 5, StudentName = "Nguyễn Hoàng Dũng", Email = "dung.nh@educenter.vn", CourseId = 2, CourseName = "IELTS 6.5+", TotalFee = 8500000, RemainingAmount = 8500000, DaysOverdue = 0, Status = "Overdue", DueDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Debt { Id = 3, StudentId = 7, StudentName = "Trần Văn Khoa", Email = "khoa.tv@educenter.vn", CourseId = 5, CourseName = "Thiết Kế Đồ Họa", TotalFee = 3000000, RemainingAmount = 3000000, DaysOverdue = 4, Status = "Warning", DueDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Debt { Id = 4, StudentId = 8, StudentName = "Đỗ Thị Lan", Email = "lan.dt@educenter.vn", CourseId = 1, CourseName = "Tiếng Anh Giao Tiếp CB", TotalFee = 3500000, RemainingAmount = 2500000, DaysOverdue = 0, Status = "Warning", DueDate = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
            new Debt { Id = 5, StudentId = 4, StudentName = "Lê Minh Châu", Email = "chau.le@educenter.vn", CourseId = 7, CourseName = "Kế Toán Doanh Nghiệp", TotalFee = 4500000, RemainingAmount = 4500000, DaysOverdue = 0, Status = "Overdue", DueDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Payrolls
        modelBuilder.Entity<Payroll>().HasData(
            new Payroll { Id = 1, TeacherId = 2, TeacherName = "Trần Thị Bình", Specialty = "Tiếng Anh", TotalClasses = 3, TotalSessions = 72, BaseSalary = 18000000, Bonus = 2000000, Deduction = 0, NetSalary = 20000000, Status = "Pending", Month = 5, Year = 2026, CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Payroll { Id = 2, TeacherId = 3, TeacherName = "Phạm Đức Cường", Specialty = "Lập trình", TotalClasses = 2, TotalSessions = 48, BaseSalary = 15000000, Bonus = 1500000, Deduction = 500000, NetSalary = 16000000, Status = "Pending", Month = 5, Year = 2026, CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
