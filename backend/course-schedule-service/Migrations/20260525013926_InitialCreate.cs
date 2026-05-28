using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseAndScheduleService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    EnrolledStudents = table.Column<int>(type: "int", nullable: false),
                    Instructor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseName", "CreatedAt", "Credits", "Description", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "C# Basics", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7601), 3, "Khóa h?c c? b?n v? C# và .NET", "Active", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7601) },
                    { 2, "Advanced ASP.NET Core", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7604), 4, "Khóa h?c nâng cao v? ASP.NET Core Web API", "Active", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7605) }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "ClassCode", "CourseId", "CreatedAt", "EnrolledStudents", "Instructor", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 30, "CS001", 1, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7764), 25, "Nguy?n V?n A", "Active", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7765) },
                    { 2, 35, "CS002", 1, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7769), 28, "Tr?n Th? B", "Active", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7770) },
                    { 3, 25, "ASP001", 2, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7772), 20, "Lê V?n C", "Active", new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7773) }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ClassDate", "ClassId", "CreatedAt", "EndTime", "Room", "StartTime", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7878), new TimeSpan(0, 10, 0, 0, 0), "A101", new TimeSpan(0, 8, 0, 0, 0), "Active", null },
                    { 2, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7882), new TimeSpan(0, 10, 0, 0, 0), "A101", new TimeSpan(0, 8, 0, 0, 0), "Active", null },
                    { 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 5, 25, 8, 39, 25, 982, DateTimeKind.Local).AddTicks(7884), new TimeSpan(0, 16, 0, 0, 0), "B202", new TimeSpan(0, 14, 0, 0, 0), "Active", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ClassId",
                table: "Schedules",
                column: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
