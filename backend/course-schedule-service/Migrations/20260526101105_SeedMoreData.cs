using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseAndScheduleService.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Văn A", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "Trần Thị B", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "Lê Văn C", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "Khóa học cơ bản về C# và .NET", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), "Khóa học nâng cao về ASP.NET Core Web API", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "CreatedAt", "Credits", "Description", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, "Flutter Mobile Dev", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 3, "Phát triển ứng dụng di động đa nền tảng với Flutter", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "AI & Machine Learning", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 4, "Ứng dụng AI/ML trong phân tích dữ liệu và dự đoán", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Odoo ERP Framework", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 3, "Quản trị doanh nghiệp và lập trình module với Odoo", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ClassDate", "ClassId", "CreatedAt", "EndTime", "Room", "StartTime", "Status", "UpdatedAt" },
                values: new object[] { 4, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), "C303", new TimeSpan(0, 9, 0, 0, 0), "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "ClassCode", "CourseId", "CreatedAt", "EnrolledStudents", "Instructor", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 4, 40, "FLT001", 3, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 38, "Phạm Đình D", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 20, "AIML01", 4, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 15, "Hoàng Anh E", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 30, "ODOO01", 5, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), 10, "Vũ Thị F", "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ClassDate", "ClassId", "CreatedAt", "EndTime", "Room", "StartTime", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 16, 0, 0, 0), "Lab-1", new TimeSpan(0, 13, 0, 0, 0), "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 16, 0, 0, 0), "Lab-1", new TimeSpan(0, 13, 0, 0, 0), "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), "Lab-AI", new TimeSpan(0, 8, 0, 0, 0), "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 0, 0, 0), "D404", new TimeSpan(0, 14, 0, 0, 0), "Active", new DateTime(2024, 5, 26, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7764), "Nguy?n V?n A", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7769), "Tr?n Th? B", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Instructor", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7772), "Lê V?n C", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7773) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7601), "Khóa h?c c? b?n v? C# và .NET", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7601) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7604), "Khóa h?c nâng cao v? ASP.NET Core Web API", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7605) });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7878), null });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7882), null });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ClassDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7884), null });
        }
    }
}
