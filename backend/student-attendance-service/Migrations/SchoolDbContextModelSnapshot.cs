using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentAttendanceService.Data;

#nullable disable

namespace StudentAttendanceService.Migrations;

[DbContext(typeof(SchoolDbContext))]
partial class SchoolDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("StudentAttendanceService.Models.Attendance", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<DateTime>("Date")
                .HasColumnType("datetime2");

            b.Property<bool>("IsPresent")
                .HasColumnType("bit");

            b.Property<string>("Note")
                .HasMaxLength(250)
                .HasColumnType("nvarchar(250)");

            b.Property<int>("ScheduleId")
                .HasColumnType("int");

            b.Property<int>("StudentId")
                .HasColumnType("int");

            b.HasKey("Id");

            b.HasIndex("StudentId");

            b.ToTable("Attendances");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Result", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("CourseId")
                .HasColumnType("int");

            b.Property<string>("Note")
                .HasMaxLength(250)
                .HasColumnType("nvarchar(250)");

            b.Property<DateTime>("ResultDate")
                .HasColumnType("datetime2");

            b.Property<decimal>("Score")
                .HasPrecision(5, 2)
                .HasColumnType("decimal(5,2)");

            b.Property<int>("StudentId")
                .HasColumnType("int");

            b.HasKey("Id");

            b.HasIndex("CourseId");
            b.HasIndex("StudentId");

            b.ToTable("Results");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Enrollment", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("CourseId")
                .HasColumnType("int");

            b.Property<DateTime>("EnrolledDate")
                .HasColumnType("datetime2");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<int>("StudentId")
                .HasColumnType("int");

            b.HasKey("Id");

            b.HasIndex("StudentId");

            b.ToTable("Enrollments");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Student", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Address")
                .HasMaxLength(250)
                .HasColumnType("nvarchar(250)");

            b.Property<DateTime>("DateOfBirth")
                .HasColumnType("datetime2");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<DateTime>("EnrolledAt")
                .HasColumnType("datetime2");

            b.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<string>("Phone")
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.HasKey("Id");

            b.HasIndex("Email")
                .IsUnique();

            b.ToTable("Students");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Attendance", b =>
        {
            b.HasOne("StudentAttendanceService.Models.Student", "Student")
                .WithMany("Attendances")
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Student");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Result", b =>
        {
            b.HasOne("StudentAttendanceService.Models.Student", "Student")
                .WithMany("Results")
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Student");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Enrollment", b =>
        {
            b.HasOne("StudentAttendanceService.Models.Student", "Student")
                .WithMany("Enrollments")
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Student");
        });

        modelBuilder.Entity("StudentAttendanceService.Models.Student", b =>
        {
            b.Navigation("Attendances");
            b.Navigation("Enrollments");
            b.Navigation("Results");
        });
#pragma warning restore 612, 618
    }
}
