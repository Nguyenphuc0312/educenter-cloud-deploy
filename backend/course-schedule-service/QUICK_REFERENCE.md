# ? Quick Reference - Schedule API

## ?? Base URL
```
https://localhost:7xxx/api/schedules
```

---

## ?? Quick Commands

### **1. Get All Schedules**
```bash
curl -X GET "https://localhost:7xxx/api/schedules" \
  -H "Accept: application/json"
```

### **2. Get Schedule by ID**
```bash
curl -X GET "https://localhost:7xxx/api/schedules/1" \
  -H "Accept: application/json"
```

### **3. Get Schedules by Class**
```bash
curl -X GET "https://localhost:7xxx/api/schedules/class/1" \
  -H "Accept: application/json"
```

### **4. Get Schedules by Date**
```bash
curl -X GET "https://localhost:7xxx/api/schedules/date/2024-01-15" \
  -H "Accept: application/json"
```

### **5. Create Schedule**
```bash
curl -X POST "https://localhost:7xxx/api/schedules" \
  -H "Content-Type: application/json" \
  -d '{
    "classID": 1,
    "room": "A101",
    "classDate": "2024-02-15T00:00:00",
    "startTime": "08:00:00",
    "endTime": "10:00:00",
    "status": "Active"
  }'
```

### **6. Update Schedule**
```bash
curl -X PUT "https://localhost:7xxx/api/schedules/1" \
  -H "Content-Type: application/json" \
  -d '{
    "scheduleID": 1,
    "classID": 1,
    "room": "A102",
    "classDate": "2024-02-15T00:00:00",
    "startTime": "09:00:00",
    "endTime": "11:00:00",
    "status": "Active"
  }'
```

### **7. Delete Schedule**
```bash
curl -X DELETE "https://localhost:7xxx/api/schedules/1"
```

---

## ?? Project Structure

```
Services/
??? Interfaces/
?   ??? IScheduleService.cs           ? Service contract
??? Implementations/
    ??? ScheduleService.cs             ? Business logic

DTOs/
??? ScheduleDto.cs                     ? Data transfer objects

Controllers/
??? SchedulesController.cs             ? API endpoints

Models/
??? Schedule.cs                        ? Database entity

Data/
??? EduCenterDbContext.cs              ? Database context
```

---

## ?? Key Files Modified/Created

| File | Status | Mô T? |
|------|--------|-------|
| `Services/Interfaces/IScheduleService.cs` | ? Created | Service interface |
| `Services/Implementations/ScheduleService.cs` | ? Created | Business logic |
| `DTOs/ScheduleDto.cs` | ? Created | Data transfer objects |
| `Controllers/SchedulesController.cs` | ? Created | API endpoints |
| `Models/Schedule.cs` | ? Updated | Database entity |
| `Data/EduCenterDbContext.cs` | ? Updated | DbContext config |
| `Program.cs` | ? Updated | Dependency injection |

---

## ?? Setup Steps

### **1. Update Connection String** (appsettings.json)
```json
"ConnectionStrings": {
  "EduCenterConnection": "Server=YOUR_SERVER;Database=EduCenter_CourseDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### **2. Create Database Migration**
```powershell
Add-Migration UpdateScheduleSchema
Update-Database
```

### **3. Build & Run**
```powershell
dotnet build
dotnet run
```

### **4. Test API**
- Open Swagger: `https://localhost:7xxx/swagger/index.html`
- Or use cURL commands above

---

## ?? API Endpoints Summary

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/schedules` | Get all schedules |
| GET | `/api/schedules/{id}` | Get schedule by ID |
| GET | `/api/schedules/class/{classId}` | Get by class |
| GET | `/api/schedules/date/{date}` | Get by date |
| POST | `/api/schedules` | Create schedule |
| PUT | `/api/schedules/{id}` | Update schedule |
| DELETE | `/api/schedules/{id}` | Delete schedule |

---

## ? Validation Rules

- ? `StartTime` < `EndTime`
- ? `ClassDate` ? Today
- ? `ClassID` must exist
- ? All fields required (except `UpdatedAt`)

---

## ?? Response Format

### ? Success
```json
{
  "success": true,
  "message": "L?y danh sách l?ch h?c thành công",
  "data": [...]
}
```

### ? Error
```json
{
  "success": false,
  "message": "L?ch h?c không t?n t?i",
  "data": null
}
```

---

## ?? DTO Fields

### **ScheduleDto** (Response)
```
- scheduleID: int
- classID: int
- room: string
- classDate: DateTime
- startTime: TimeSpan
- endTime: TimeSpan
- status: string
- createdAt: DateTime
- updatedAt: DateTime?
- class: ClassDto
```

### **CreateScheduleDto** (Request)
```
- classID: int (required)
- room: string
- classDate: DateTime (required)
- startTime: TimeSpan (required)
- endTime: TimeSpan (required)
- status: string (optional, default: "Active")
```

### **UpdateScheduleDto** (Request)
```
- scheduleID: int (required)
- classID: int (required)
- room: string
- classDate: DateTime (required)
- startTime: TimeSpan (required)
- endTime: TimeSpan (required)
- status: string
```

---

## ?? Test in Postman

1. **Import Collection**
   - Swagger endpoint: `https://localhost:7xxx/swagger/v1/swagger.json`
   - Or create manually

2. **Set Variables**
   ```
   {{base_url}} = https://localhost:7xxx/api
   {{schedule_id}} = 1
   {{class_id}} = 1
   {{date}} = 2024-01-15
   ```

3. **Test Request**
   ```
   GET {{base_url}}/schedules
   ```

---

## ?? Architecture Pattern

```
User Request
    ?
[SchedulesController]        ? HTTP Handler
    ?
[IScheduleService]           ? Service Interface
    ?
[ScheduleService]            ? Business Logic
    ?
[EduCenterDbContext]         ? Data Access
    ?
[SQL Server Database]        ? Data Storage
```

---

## ?? Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| 404 Not Found | Check endpoint URL & HTTP method |
| 400 Bad Request | Validate request body JSON format |
| Connection Timeout | Check Connection String & SQL Server |
| Validation Error | Check StartTime < EndTime & ClassDate >= Today |

---

## ?? Documentation Files

- **README.md** - Project overview
- **HUONG_DAN.md** - Detailed setup guide
- **ARCHITECTURE_GUIDE.md** - Clean architecture explanation
- **MIGRATION_GUIDE.md** - Database migration instructions
- **API_TEST_EXAMPLES.md** - API testing examples
- **FRONTEND_INTEGRATION.md** - Frontend integration guide

---

**Happy Coding! ??**
