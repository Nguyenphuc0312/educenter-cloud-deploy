# ?? Course & Schedule Microservice - ASP.NET Core Web API

[![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)]()
[![API Version](https://img.shields.io/badge/API-v1.0-blue)]()
[![Database](https://img.shields.io/badge/Database-SQL%20Server-orange)]()
[![Status](https://img.shields.io/badge/Status-Active-brightgreen)]()

## ?? Mô T? D? Án

**Course & Schedule Service** là m?t Microservice ???c xây d?ng b?ng ASP.NET Core 8.0 v?i Entity Framework Core, k?t n?i SQL Server. Service này cung c?p các API ?? qu?n lý:

- ?? **Courses** (Khóa H?c)
- ?? **Classes** (L?p H?c)
- ?? **Schedules** (L?ch H?c)

---

## ? Tính N?ng Chính

### ? CRUD Operations ??y ??
- T?o, ??c, c?p nh?t, xóa khóa h?c, l?p, và l?ch h?c

### ? Validation
- Ki?m tra ràng bu?c Foreign Key
- Ki?m tra th?i gian h?p l? (Start < End)
- Ki?m tra d? li?u b?t bu?c

### ? Endpoints Linh Ho?t
- L?y danh sách theo filter (ví d?: l?p c?a khóa h?c, l?ch c?a ngày)
- Include navigation properties cho d? li?u liên quan

### ? Response Format Chung
T?t c? API tr? v? format JSON chu?n:
```json
{
  "success": true/false,
  "message": "...",
  "data": {...}
}
```

### ? CORS Enabled
- Cho phép Frontend t? b?t k? origin nào g?i API

### ? Documentation
- Swagger/OpenAPI integration s?n
- Extensive API documentation

---

## ??? Ki?n Trúc

```
CourseAndScheduleService/
??? Models/
?   ??? Course.cs              # Entity - Khóa H?c
?   ??? Class.cs               # Entity - L?p H?c
?   ??? Schedule.cs            # Entity - L?ch H?c
?
??? Data/
?   ??? EduCenterDbContext.cs  # Entity Framework DbContext
?
??? Controllers/
?   ??? CoursesController.cs    # API Courses
?   ??? ClassesController.cs    # API Classes
?   ??? SchedulesController.cs  # API Schedules
?
??? Responses/
?   ??? ApiResponse.cs         # Response model chung
?
??? Program.cs                  # Dependency Injection & Middleware
??? appsettings.json           # Configuration
??? CourseAndScheduleService.csproj
```

---

## ??? Database Schema

### Courses Table
```sql
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Credits INT NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);
```

### Classes Table
```sql
CREATE TABLE Classes (
    ClassID INT PRIMARY KEY IDENTITY(1,1),
    ClassCode NVARCHAR(50) NOT NULL,
    CourseID INT NOT NULL,
    Capacity INT NOT NULL,
    EnrolledStudents INT DEFAULT 0,
    Instructor NVARCHAR(100),
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE
);
```

### Schedules Table
```sql
CREATE TABLE Schedules (
    ScheduleID INT PRIMARY KEY IDENTITY(1,1),
    ClassID INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    DayOfWeek NVARCHAR(10) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Room NVARCHAR(100),
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID) ON DELETE CASCADE
);
```

### Quan H?
```
Courses (1) ??? (N) Classes ??? (N) Schedules
  1:N              1:N
```

---

## ?? Quick Start

### Prerequisite
- .NET 8.0 SDK
- SQL Server (2019 ho?c m?i h?n)
- Visual Studio 2022 ho?c VS Code

### Installation & Configuration

**1. Update Connection String**
```json
// appsettings.json
"ConnectionStrings": {
  "EduCenterConnection": "Server=YOUR_SERVER;Database=EduCenter_CourseDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

**2. Create Database & Tables**
```powershell
# Package Manager Console
Add-Migration InitialCreate
Update-Database
```

**3. Run Application**
```powershell
# Terminal
dotnet run

# ho?c nh?n F5 trong Visual Studio
```

**4. Access Swagger UI**
```
https://localhost:7xxx/swagger/index.html
```

---

## ?? API Endpoints

### ?? Courses API
```
GET    /api/courses                 # L?y t?t c? khóa h?c
GET    /api/courses/{id}            # L?y khóa h?c theo ID
POST   /api/courses                 # T?o khóa h?c m?i
PUT    /api/courses/{id}            # C?p nh?t khóa h?c
DELETE /api/courses/{id}            # Xóa khóa h?c
```

### ?? Classes API
```
GET    /api/classes                 # L?y t?t c? l?p
GET    /api/classes/{id}            # L?y l?p theo ID
GET    /api/classes/course/{courseId}  # L?y l?p c?a khóa h?c
POST   /api/classes                 # T?o l?p m?i
PUT    /api/classes/{id}            # C?p nh?t l?p
DELETE /api/classes/{id}            # Xóa l?p
```

### ?? Schedules API
```
GET    /api/schedules               # L?y t?t c? l?ch
GET    /api/schedules/{id}          # L?y l?ch theo ID
GET    /api/schedules/class/{classId}    # L?y l?ch c?a l?p
GET    /api/schedules/day/{dayOfWeek}    # L?y l?ch theo ngày
POST   /api/schedules               # T?o l?ch m?i
PUT    /api/schedules/{id}          # C?p nh?t l?ch
DELETE /api/schedules/{id}          # Xóa l?ch
```

---

## ?? Example Requests

### Create Course
```bash
POST /api/courses
Content-Type: application/json

{
  "courseName": "Advanced C#",
  "description": "Learn advanced C# concepts",
  "credits": 4,
  "status": "Active"
}
```

### Get Classes by Course
```bash
GET /api/classes/course/1
```

### Create Schedule
```bash
POST /api/schedules
Content-Type: application/json

{
  "classID": 1,
  "startDate": "2024-02-01T00:00:00",
  "endDate": "2024-06-01T00:00:00",
  "dayOfWeek": "Monday",
  "startTime": "08:00:00",
  "endTime": "10:00:00",
  "room": "A101",
  "status": "Active"
}
```

---

## ?? Technologies Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 8.0 | Framework |
| ASP.NET Core | 8.0 | Web Framework |
| Entity Framework Core | 8.0 | ORM |
| SQL Server | 2019+ | Database |
| Swagger | 6.6.2 | API Documentation |

---

## ?? NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

---

## ?? Configuration

### CORS Configuration
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### DbContext Configuration
```csharp
builder.Services.AddDbContext<EduCenterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EduCenterConnection"))
);
```

---

## ?? Testing

### S? d?ng Swagger UI
1. Truy c?p `https://localhost:7xxx/swagger/index.html`
2. Click vào endpoint
3. Click "Try it out"
4. Nh?p d? li?u
5. Click "Execute"

### S? d?ng cURL
```bash
# L?y danh sách khóa h?c
curl https://localhost:7xxx/api/courses \
  -H "Accept: application/json"

# T?o khóa h?c m?i
curl -X POST https://localhost:7xxx/api/courses \
  -H "Content-Type: application/json" \
  -d '{"courseName":"New Course","description":"Desc","credits":3,"status":"Active"}'
```

### S? d?ng Postman
- Import collection t? Swagger endpoint
- Ho?c t?o request b?ng tay

---

## ?? Security Notes

### ?? Current State (Development)
- CORS m? toàn b? (cho development)
- Không có JWT authentication (t?m th?i)
- Không có rate limiting

### ? Future Improvements
- [ ] Thêm JWT Authentication
- [ ] Implement Role-based Authorization
- [ ] Add API Key validation
- [ ] Enable rate limiting
- [ ] Implement logging & monitoring
- [ ] Add input sanitization
- [ ] Enable HTTPS only in production

---

## ?? Documentation Files

- **`HUONG_DAN.md`** - H??ng d?n chi ti?t setup & configuration
- **`API_TEST_EXAMPLES.md`** - Ví d? test API b?ng cURL, Postman, Thunder Client
- **`FRONTEND_INTEGRATION.md`** - H??ng d?n tích h?p t? Frontend (React, Angular, Vue)

---

## ?? Troubleshooting

### Connection String Issues
```
? Error: "Connection timeout"
? Solution: 
  - Ki?m tra Server name
  - ??m b?o SQL Server ?ang ch?y
  - Ki?m tra firewall settings
```

### Migration Issues
```
? Error: "Migration failed"
? Solution:
  - Xóa folder Migrations (n?u t?n t?i)
  - Ch?y: Add-Migration InitialCreate
  - Ch?y: Update-Database
```

### CORS Issues
```
? Error: "Cross-Origin Request Blocked"
? Solution:
  - CORS ?ã ???c enable trong Program.cs
  - Ki?m tra frontend URL
  - Clear browser cache
```

---

## ?? Support

N?u g?p v?n ??:
1. Ki?m tra l?i các file documentation
2. Xem thêm trong Output window c?a Visual Studio
3. Ki?m tra SQL Server Management Studio

---

## ?? License

Educational purposes only

---

## ?? K?t Lu?n

Project này cung c?p n?n t?ng v?ng ch?c cho vi?c phát tri?n Microservice qu?n lý khóa h?c và l?ch h?c. B?n có th? m? r?ng b?ng cách:

- ? Thêm Authentication & Authorization
- ? Thêm caching (Redis)
- ? Thêm logging (Serilog)
- ? Thêm event publishing (RabbitMQ, Kafka)
- ? Containerize v?i Docker
- ? Deploy lên Kubernetes

---

**Happy Coding! ??**

Made with ?? for Educational Purposes
