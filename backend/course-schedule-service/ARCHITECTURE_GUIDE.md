# ??? Clean Architecture - Schedule Service Implementation Guide

## ?? Tóm T?t Nh?ng Gì ?ã Th?c Hi?n

Tôi ?ã refactor toàn b? Microservice theo **Clean Architecture Pattern** v?i các l?p sau:

```
???????????????????????????????????????
?        Controllers Layer             ? (API Endpoints)
?  - SchedulesController              ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      Services/Business Logic         ? (Business Rules)
?  - IScheduleService (Interface)      ?
?  - ScheduleService (Implementation)  ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      Data Access Layer               ? (Database)
?  - EduCenterDbContext                ?
?  - Schedule Entity                   ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?      DTOs & Models                   ? (Data Transfer)
?  - ScheduleDto                       ?
?  - CreateScheduleDto                 ?
?  - UpdateScheduleDto                 ?
???????????????????????????????????????
```

---

## ?? C?u Trúc File M?i

```
CourseAndScheduleService/
??? Controllers/
?   ??? SchedulesController.cs              ? API Endpoints
??? Services/
?   ??? Interfaces/
?   ?   ??? IScheduleService.cs             ? Service Contract
?   ??? Implementations/
?       ??? ScheduleService.cs              ? Business Logic
??? DTOs/
?   ??? ScheduleDto.cs                      ? Data Transfer Objects
??? Models/
?   ??? Schedule.cs                         ? Database Entity (Updated)
??? Data/
?   ??? EduCenterDbContext.cs               ? Database Context (Updated)
??? Responses/
?   ??? ApiResponse.cs                      (Unchanged)
??? Program.cs                              ? DI Configuration (Updated)
```

---

## ?? Chi Ti?t Các Thành Ph?n

### **1. DTOs (DTOs/ScheduleDto.cs)**

```csharp
// ?? tr? v? data t? API
public class ScheduleDto
{
    public int ScheduleID { get; set; }
    public int ClassID { get; set; }
    public string Room { get; set; }
    public DateTime ClassDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ClassDto? Class { get; set; }  // Navigation
}

// ?? t?o Schedule m?i
public class CreateScheduleDto
{
    public int ClassID { get; set; }
    public string Room { get; set; }
    public DateTime ClassDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}

// ?? c?p nh?t Schedule
public class UpdateScheduleDto
{
    public int ScheduleID { get; set; }
    public int ClassID { get; set; }
    public string Room { get; set; }
    public DateTime ClassDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}
```

**?? L?i Ích:**
- ? Tách bi?t Entity t? API Response
- ? Có th? return/receive khác nhau t?/??n DB
- ? D? version API sau này

---

### **2. Service Interface (Services/Interfaces/IScheduleService.cs)**

```csharp
public interface IScheduleService
{
    Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
    Task<ScheduleDto?> GetScheduleByIdAsync(int id);
    Task<IEnumerable<ScheduleDto>> GetSchedulesByClassIdAsync(int classId);
    Task<IEnumerable<ScheduleDto>> GetSchedulesByDateAsync(DateTime classDate);
    Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto);
    Task<ScheduleDto> UpdateScheduleAsync(UpdateScheduleDto updateScheduleDto);
    Task<bool> DeleteScheduleAsync(int id);
}
```

**?? L?i Ích:**
- ? ??nh ngh?a rõ ràng các operation
- ? D? unit test (mock interface)
- ? Loose coupling gi?a Controller và Service

---

### **3. Service Implementation (Services/Implementations/ScheduleService.cs)**

```csharp
public class ScheduleService : IScheduleService
{
    private readonly EduCenterDbContext _context;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(EduCenterDbContext context, ILogger<ScheduleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Implement các method t? interface
    public async Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto)
    {
        // Validation
        ValidateScheduleData(...);
        
        // Business logic
        var schedule = new Schedule { ... };
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        
        return MapToDto(schedule);
    }
    
    private void ValidateScheduleData(...) { }
    private ScheduleDto MapToDto(Schedule schedule) { }
}
```

**?? ?i?m Quan Tr?ng:**
- ? Ch?a t?t c? business logic
- ? Validation d? li?u
- ? Mapping Entity ? DTO
- ? Error handling & logging

---

### **4. Controller (Controllers/SchedulesController.cs)**

```csharp
[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<SchedulesController> _logger;

    public SchedulesController(IScheduleService scheduleService, 
                              ILogger<SchedulesController> logger)
    {
        _scheduleService = scheduleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleDto>>>> 
        GetAllSchedules()
    {
        var schedules = await _scheduleService.GetAllSchedulesAsync();
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.SuccessResponse(
            "L?y danh sách l?ch h?c thành công", schedules));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ScheduleDto>>> 
        CreateSchedule([FromBody] CreateScheduleDto createScheduleDto)
    {
        var schedule = await _scheduleService.CreateScheduleAsync(createScheduleDto);
        return CreatedAtAction(nameof(GetScheduleById), 
            new { id = schedule.ScheduleID }, 
            ApiResponse<ScheduleDto>.SuccessResponse("T?o l?ch h?c thành công", schedule));
    }
    // ... other endpoints
}
```

**?? ?i?m Quan Tr?ng:**
- ? Ch? x? lý HTTP concerns
- ? G?i Service ?? x? lý logic
- ? Return ApiResponse format chung
- ? Error handling & logging

---

## ?? Dependency Injection (Program.cs)

```csharp
// Thêm dòng này vào Program.cs
builder.Services.AddScoped<IScheduleService, ScheduleService>();
```

**?? Gi?i Thích `AddScoped`:**
- **Scoped**: T?o 1 instance m?i cho m?i HTTP request
- **Singleton**: Dùng 1 instance cho toàn b? app
- **Transient**: T?o instance m?i m?i l?n

---

## ?? Các API Endpoint

### **GET - L?y T?t C?**
```
GET /api/schedules
```

**Response:**
```json
{
  "success": true,
  "message": "L?y danh sách l?ch h?c thành công",
  "data": [
    {
      "scheduleID": 1,
      "classID": 1,
      "room": "A101",
      "classDate": "2024-01-15T00:00:00",
      "startTime": "08:00:00",
      "endTime": "10:00:00",
      "status": "Active",
      "createdAt": "2024-01-10T10:30:00",
      "updatedAt": null,
      "class": { ... }
    }
  ]
}
```

### **GET - L?y Theo ID**
```
GET /api/schedules/1
```

### **GET - L?y Theo L?p**
```
GET /api/schedules/class/1
```

### **GET - L?y Theo Ngày**
```
GET /api/schedules/date/2024-01-15
```

### **POST - T?o M?i**
```
POST /api/schedules
Content-Type: application/json

{
  "classID": 1,
  "room": "A101",
  "classDate": "2024-02-15T00:00:00",
  "startTime": "08:00:00",
  "endTime": "10:00:00",
  "status": "Active"
}
```

### **PUT - C?p Nh?t**
```
PUT /api/schedules/1
Content-Type: application/json

{
  "scheduleID": 1,
  "classID": 1,
  "room": "A102",
  "classDate": "2024-02-15T00:00:00",
  "startTime": "09:00:00",
  "endTime": "11:00:00",
  "status": "Active"
}
```

### **DELETE - Xóa**
```
DELETE /api/schedules/1
```

---

## ? Validation Logic

Service ?ã implement các validation sau:

### **1. Validation Th?i Gian**
```csharp
if (startTime >= endTime)
    throw new ArgumentException("Th?i gian b?t ??u ph?i tr??c th?i gian k?t thúc");
```

### **2. Validation Ngày**
```csharp
if (classDate < DateTime.Today)
    throw new ArgumentException("Ngày h?c không th? là ngày quá kh?");
```

### **3. Validation Foreign Key**
```csharp
var classExists = await _context.Classes.AnyAsync(c => c.ClassID == classId);
if (!classExists)
    throw new ArgumentException($"L?p v?i ID {classId} không t?n t?i");
```

---

## ?? Database Migration

Khi schema Schedule thay ??i, c?n t?o migration m?i:

```powershell
# Package Manager Console
Add-Migration UpdateScheduleSchema
Update-Database
```

**Các field m?i/thay ??i:**
- ? `StartDate` + `EndDate` ? `ClassDate` (??n gi?n hóa)
- ? `DayOfWeek` (b? xóa, vì `ClassDate` ?ã có ngày)
- ? `CreatedDate` ? `CreatedAt`
- ? `UpdatedDate` ? `UpdatedAt` (nullable)

---

## ?? Unit Test Example

```csharp
[TestClass]
public class ScheduleServiceTests
{
    private ScheduleService _scheduleService;
    private Mock<EduCenterDbContext> _mockContext;
    private Mock<ILogger<ScheduleService>> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        _mockContext = new Mock<EduCenterDbContext>();
        _mockLogger = new Mock<ILogger<ScheduleService>>();
        _scheduleService = new ScheduleService(_mockContext.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task CreateSchedule_WithValidData_ShouldReturnScheduleDto()
    {
        // Arrange
        var createDto = new CreateScheduleDto
        {
            ClassID = 1,
            Room = "A101",
            ClassDate = DateTime.Now.AddDays(5),
            StartTime = new TimeSpan(8, 0, 0),
            EndTime = new TimeSpan(10, 0, 0),
            Status = "Active"
        };

        // Act
        var result = await _scheduleService.CreateScheduleAsync(createDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("A101", result.Room);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task CreateSchedule_WithInvalidTime_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateScheduleDto
        {
            ClassID = 1,
            Room = "A101",
            ClassDate = DateTime.Now.AddDays(5),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(8, 0, 0),  // EndTime < StartTime
            Status = "Active"
        };

        // Act
        await _scheduleService.CreateScheduleAsync(createDto);
    }
}
```

---

## ?? L?i Ích C?a Clean Architecture

| L?i Ích | Gi?i Thích |
|---------|-----------|
| **Separation of Concerns** | M?i l?p có trách nhi?m riêng |
| **Testability** | D? unit test vì có interface |
| **Maintainability** | D? s?a/update code |
| **Reusability** | Service có th? reuse ? nhi?u n?i |
| **Scalability** | D? m? r?ng khi thêm features |
| **Loose Coupling** | Controller không ph? thu?c vào DB |

---

## ?? Checklist Khi Thêm Feature M?i

N?u mu?n thêm CRUD cho Entity khác (ví d?: Teacher), làm theo steps này:

- [ ] **T?o DTO files** (CreateTeacherDto, UpdateTeacherDto, TeacherDto)
- [ ] **T?o Interface** (ITeacherService)
- [ ] **Implement Service** (TeacherService)
- [ ] **Register trong DI** (builder.Services.AddScoped<ITeacherService, TeacherService>())
- [ ] **T?o Controller** (TeachersController)
- [ ] **Test API** v?i Swagger

---

## ?? Ti?p Theo

Có th? thêm:
- [ ] DTOs cho Course & Classes (refactor t??ng t?)
- [ ] Interfaces & Services cho Course & Classes
- [ ] Unit tests
- [ ] AutoMapper (?? mapping Entity ? DTO t? ??ng)
- [ ] FluentValidation (validation library)
- [ ] Pagination & Filtering
- [ ] JWT Authentication

---

**Build Successful! ?**

C?u trúc project gi? ?ây tuân theo Clean Architecture Pattern, d? maintain và scale.
