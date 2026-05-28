using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentAttendanceService.Data;

#nullable disable

namespace StudentAttendanceService.Migrations;

[DbContext(typeof(SchoolDbContext))]
[Migration("20260524000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Attendances",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<int>(type: "int", nullable: false),
                ScheduleId = table.Column<int>(type: "int", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsPresent = table.Column<bool>(type: "bit", nullable: false),
                Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attendances", x => x.Id);
                table.ForeignKey(
                    name: "FK_Attendances_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Enrollments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<int>(type: "int", nullable: false),
                CourseId = table.Column<int>(type: "int", nullable: false),
                EnrolledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enrollments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Enrollments_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Attendances_StudentId",
            table: "Attendances",
            column: "StudentId");

        migrationBuilder.CreateIndex(
            name: "IX_Enrollments_StudentId",
            table: "Enrollments",
            column: "StudentId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_Email",
            table: "Students",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Attendances");
        migrationBuilder.DropTable(name: "Enrollments");
        migrationBuilder.DropTable(name: "Students");
    }
}
