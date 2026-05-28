using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaymentService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Debts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DaysOverdue = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalClasses = table.Column<int>(type: "int", nullable: false),
                    TotalSessions = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Debts",
                columns: new[] { "Id", "CourseId", "CourseName", "CreatedAt", "DaysOverdue", "DueDate", "Email", "RemainingAmount", "Status", "StudentId", "StudentName", "TotalFee" },
                values: new object[,]
                {
                    { 1, 3, "Lập Trình Python CB", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 10, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "chau.le@educenter.vn", 2000000m, "Warning", 4, "Lê Minh Châu", 4000000m },
                    { 2, 2, "IELTS 6.5+", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dung.nh@educenter.vn", 8500000m, "Overdue", 5, "Nguyễn Hoàng Dũng", 8500000m },
                    { 3, 5, "Thiết Kế Đồ Họa", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "khoa.tv@educenter.vn", 3000000m, "Warning", 7, "Trần Văn Khoa", 3000000m },
                    { 4, 1, "Tiếng Anh Giao Tiếp CB", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "lan.dt@educenter.vn", 2500000m, "Warning", 8, "Đỗ Thị Lan", 3500000m },
                    { 5, 7, "Kế Toán Doanh Nghiệp", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "chau.le@educenter.vn", 4500000m, "Overdue", 4, "Lê Minh Châu", 4500000m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "CourseId", "CourseName", "CreatedAt", "DueDate", "Note", "PaidAmount", "PaymentDate", "PaymentMethod", "Status", "StudentId", "StudentName", "TransactionCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 3500000m, 1, "Tiếng Anh Giao Tiếp CB", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 3500000m, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), "BankTransfer", "Paid", 4, "Lê Minh Châu", "TXN001", null },
                    { 2, 4000000m, 3, "Lập Trình Python CB", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 2000000m, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cash", "Partial", 4, "Lê Minh Châu", null, null },
                    { 3, 8500000m, 2, "IELTS 6.5+", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, null, "Unpaid", 5, "Nguyễn Hoàng Dũng", null, null },
                    { 4, 12000000m, 4, "Lập Trình Web Full-Stack", new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 12000000m, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), "VNPay", "Paid", 6, "Phạm Thị Hà", "TXN002", null },
                    { 5, 3000000m, 5, "Thiết Kế Đồ Họa", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, null, "Unpaid", 7, "Trần Văn Khoa", null, null },
                    { 6, 3500000m, 1, "Tiếng Anh Giao Tiếp CB", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 1000000m, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "MoMo", "Partial", 8, "Đỗ Thị Lan", null, null },
                    { 7, 6000000m, 6, "Digital Marketing", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 6000000m, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "BankTransfer", "Paid", 5, "Nguyễn Hoàng Dũng", "TXN003", null },
                    { 8, 4500000m, 7, "Kế Toán Doanh Nghiệp", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, 0m, null, null, "Unpaid", 4, "Lê Minh Châu", null, null }
                });

            migrationBuilder.InsertData(
                table: "Payrolls",
                columns: new[] { "Id", "BaseSalary", "Bonus", "CreatedAt", "Deduction", "Month", "NetSalary", "PaidDate", "Specialty", "Status", "TeacherId", "TeacherName", "TotalClasses", "TotalSessions", "Year" },
                values: new object[,]
                {
                    { 1, 18000000m, 2000000m, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0m, 5, 20000000m, null, "Tiếng Anh", "Pending", 2, "Trần Thị Bình", 3, 72, 2026 },
                    { 2, 15000000m, 1500000m, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 500000m, 5, 16000000m, null, "Lập trình", "Pending", 3, "Phạm Đức Cường", 2, 48, 2026 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "Password", "Phone", "Role", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@educenter.vn", "Nguyễn Văn An", "admin123", "0900000001", "admin", "Active", null },
                    { 2, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "binh.tran@educenter.vn", "Trần Thị Bình", "teacher123", "0901234567", "teacher", "Active", null },
                    { 3, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cuong.pham@educenter.vn", "Phạm Đức Cường", "teacher123", "0912345678", "teacher", "Active", null },
                    { 4, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "chau.le@educenter.vn", "Lê Minh Châu", "student123", "0901111111", "student", "Active", null },
                    { 5, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "dung.nh@educenter.vn", "Nguyễn Hoàng Dũng", "student123", "0902222222", "student", "Active", null },
                    { 6, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "ha.pt@educenter.vn", "Phạm Thị Hà", "student123", "0903333333", "student", "Active", null },
                    { 7, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "khoa.tv@educenter.vn", "Trần Văn Khoa", "student123", "0904444444", "student", "Active", null },
                    { 8, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "lan.dt@educenter.vn", "Đỗ Thị Lan", "student123", "0905555555", "student", "Active", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
