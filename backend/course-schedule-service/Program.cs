using CourseAndScheduleService.Data;
using CourseAndScheduleService.Services.Implementations;
using CourseAndScheduleService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Thêm DbContext v?i SQL Server
builder.Services.AddDbContext<EduCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EduCenterConnection"))
);

// Dependency Injection - Thêm Services
builder.Services.AddScoped<IScheduleService, ScheduleService>();

// C?u hình CORS - M? hoàn toàn cho phép Frontend g?i API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add controllers and configure JSON to ignore object reference cycles
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm Logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// S? d?ng CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

