using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentAttendanceService.Data;

#nullable disable

namespace StudentAttendanceService.Migrations;

[DbContext(typeof(SchoolDbContext))]
[Migration("20260524000001_AddResults")]
public partial class AddResults : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Results",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<int>(type: "int", nullable: false),
                CourseId = table.Column<int>(type: "int", nullable: false),
                Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                ResultDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Results", x => x.Id);
                table.ForeignKey(
                    name: "FK_Results_Students_StudentId",
                    column: x => x.StudentId,
                    principalTable: "Students",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Results_CourseId",
            table: "Results",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_Results_StudentId",
            table: "Results",
            column: "StudentId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Results");
    }
}
