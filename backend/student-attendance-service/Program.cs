using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentAttendanceService.Data;
using StudentAttendanceService.Models;
using StudentAttendanceService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Attendance Service",
        Version = "v1"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IResultService, ResultService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    await dbContext.Database.MigrateAsync();
    await SeedDataAsync(dbContext);
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Attendance Service v1");
});

app.UseCors("AllowVue");

app.MapControllers();

app.Run();

static async Task SeedDataAsync(SchoolDbContext dbContext)
{
    var seedStudents = new[]
    {
        new Student
        {
            FullName = "Nguyen Van An",
            Email = "an@gmail.com",
            Phone = "0909000001",
            DateOfBirth = new DateTime(2004, 5, 12),
            Address = "Ha Noi",
            EnrolledAt = new DateTime(2026, 1, 10),
            Status = "Active"
        },
        new Student
        {
            FullName = "Tran Thi Binh",
            Email = "binh@gmail.com",
            Phone = "0909000002",
            DateOfBirth = new DateTime(2005, 7, 20),
            Address = "Da Nang",
            EnrolledAt = new DateTime(2026, 1, 11),
            Status = "Active"
        },
        new Student
        {
            FullName = "Le Van Cuong",
            Email = "cuong@gmail.com",
            Phone = "0909000003",
            DateOfBirth = new DateTime(2003, 11, 2),
            Address = "Ho Chi Minh",
            EnrolledAt = new DateTime(2026, 1, 12),
            Status = "Inactive"
        }
    };

    foreach (var seedStudent in seedStudents)
    {
        var exists = await dbContext.Students.AnyAsync(x => x.Email == seedStudent.Email);
        if (!exists)
        {
            dbContext.Students.Add(seedStudent);
        }
    }

    await dbContext.SaveChangesAsync();

    var seedEmails = seedStudents.Select(x => x.Email).ToArray();
    var studentMap = (await dbContext.Students
            .Where(x => seedEmails.Contains(x.Email))
            .ToListAsync())
        .ToDictionary(x => x.Email, x => x, StringComparer.OrdinalIgnoreCase);

    if (!await dbContext.Enrollments.AnyAsync())
    {
        dbContext.Enrollments.AddRange(
            new Enrollment
            {
                StudentId = studentMap["an@gmail.com"].Id,
                CourseId = 101,
                EnrolledDate = new DateTime(2026, 1, 12),
                Status = "Enrolled"
            },
            new Enrollment
            {
                StudentId = studentMap["binh@gmail.com"].Id,
                CourseId = 102,
                EnrolledDate = new DateTime(2026, 1, 13),
                Status = "Enrolled"
            },
            new Enrollment
            {
                StudentId = studentMap["cuong@gmail.com"].Id,
                CourseId = 101,
                EnrolledDate = new DateTime(2026, 1, 14),
                Status = "Pending"
            });

        await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Attendances.AnyAsync())
    {
        dbContext.Attendances.AddRange(
            new Attendance
            {
                StudentId = studentMap["an@gmail.com"].Id,
                ScheduleId = 201,
                Date = new DateTime(2026, 1, 15),
                IsPresent = true,
                Note = "On time"
            },
            new Attendance
            {
                StudentId = studentMap["binh@gmail.com"].Id,
                ScheduleId = 202,
                Date = new DateTime(2026, 1, 15),
                IsPresent = false,
                Note = "Absent"
            },
            new Attendance
            {
                StudentId = studentMap["cuong@gmail.com"].Id,
                ScheduleId = 203,
                Date = new DateTime(2026, 1, 16),
                IsPresent = true,
                Note = "Late 5 minutes"
            });

        await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Results.AnyAsync())
    {
        dbContext.Results.AddRange(
            new Result
            {
                StudentId = studentMap["an@gmail.com"].Id,
                CourseId = 101,
                Score = 8.5m,
                ResultDate = new DateTime(2026, 1, 20),
                Note = "Good performance"
            },
            new Result
            {
                StudentId = studentMap["binh@gmail.com"].Id,
                CourseId = 102,
                Score = 7.0m,
                ResultDate = new DateTime(2026, 1, 21),
                Note = "Needs more practice"
            },
            new Result
            {
                StudentId = studentMap["cuong@gmail.com"].Id,
                CourseId = 101,
                Score = 9.0m,
                ResultDate = new DateTime(2026, 1, 22),
                Note = "Excellent"
            });

        await dbContext.SaveChangesAsync();
    }
}
