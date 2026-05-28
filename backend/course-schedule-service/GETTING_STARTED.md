# ?? START HERE - Getting Started in 5 Minutes

## ?? Quick Setup (5 minutes)

### **Step 1: Update Connection String** (1 min)
Open `appsettings.json`:
```json
"ConnectionStrings": {
  "EduCenterConnection": "Server=YOUR_SERVER;Database=EduCenter_CourseDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

Replace `YOUR_SERVER` with your SQL Server name:
- Local: `localhost` or `.\SQLEXPRESS`
- Remote: `IP_ADDRESS` or `hostname`

### **Step 2: Create Database** (2 min)
Open **Package Manager Console** in Visual Studio:
```powershell
Add-Migration UpdateScheduleSchema
Update-Database
```

### **Step 3: Run Application** (1 min)
```powershell
dotnet run
# or press F5 in Visual Studio
```

### **Step 4: Test API** (1 min)
Open your browser:
```
https://localhost:7xxx/swagger/index.html
```

Try endpoint: `GET /api/schedules`

---

## ? If Everything Works

You should see:
```json
{
  "success": true,
  "message": "L?y danh sách l?ch h?c thành công",
  "data": [ { ... } ]
}
```

?? **Congratulations! Your API is running!**

---

## ?? What To Read Next

### **Choose Your Role:**

????? **Developer**
? Read [QUICK_REFERENCE.md](./QUICK_REFERENCE.md) (API commands)
? Then [ARCHITECTURE_GUIDE.md](./ARCHITECTURE_GUIDE.md)

??? **Architect**
? Read [ARCHITECTURE_GUIDE.md](./ARCHITECTURE_GUIDE.md)
? Review [REFACTOR_SUMMARY.md](./REFACTOR_SUMMARY.md)

?? **Using AI/Copilot**
? Read [COPILOT_GUIDE.md](./COPILOT_GUIDE.md)

?? **Tester**
? Read [API_TEST_EXAMPLES.md](./API_TEST_EXAMPLES.md)

?? **Frontend**
? Read [FRONTEND_INTEGRATION.md](./FRONTEND_INTEGRATION.md)

??? **DevOps/Database**
? Read [MIGRATION_GUIDE.md](./MIGRATION_GUIDE.md)

---

## ?? If Something Goes Wrong

### **Error: "Connection timeout"**
```
? SQL Server not running
? Solution: 
   - Start SQL Server
   - Verify server name
   - Check firewall
```

### **Error: "Migration failed"**
```
? Database doesn't exist or schema mismatch
? Solution:
   - Delete folder: Migrations (optional)
   - Run: Update-Database -Migration 0
   - Run: Add-Migration InitialCreate
   - Run: Update-Database
```

### **Error: "404 Not Found"**
```
? Wrong endpoint or server not running
? Solution:
   - Check URL in browser/Postman
   - Verify server is running
   - Check port number (7xxx)
```

---

## ?? 7 API Endpoints You Can Test

### **1. Get All Schedules**
```bash
GET /api/schedules
```

### **2. Create Schedule**
```bash
POST /api/schedules
{
  "classID": 1,
  "room": "A101",
  "classDate": "2024-02-15T00:00:00",
  "startTime": "08:00:00",
  "endTime": "10:00:00",
  "status": "Active"
}
```

### **3. Get Schedule by ID**
```bash
GET /api/schedules/1
```

### **4. Get Schedules by Class**
```bash
GET /api/schedules/class/1
```

### **5. Get Schedules by Date**
```bash
GET /api/schedules/date/2024-01-15
```

### **6. Update Schedule**
```bash
PUT /api/schedules/1
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

### **7. Delete Schedule**
```bash
DELETE /api/schedules/1
```

---

## ?? Project Structure

```
Models/           ? Database entities
??? Course.cs
??? Class.cs
??? Schedule.cs

DTOs/             ? Data transfer objects
??? ScheduleDto.cs

Services/         ? Business logic
??? Interfaces/
?   ??? IScheduleService.cs
??? Implementations/
    ??? ScheduleService.cs

Controllers/      ? API endpoints
??? CoursesController.cs
??? ClassesController.cs
??? SchedulesController.cs

Data/             ? Database
??? EduCenterDbContext.cs

Program.cs        ? Configuration
```

---

## ?? Configuration Summary

### **Database**
- Type: SQL Server
- Name: EduCenter_CourseDB
- Tables: 3 (Courses, Classes, Schedules)

### **API**
- Base URL: https://localhost:7xxx/api
- Response Format: ApiResponse<T>
- CORS: Enabled (AllowAnyOrigin)

### **Framework**
- .NET: 8.0
- Pattern: Clean Architecture
- Services: 1 (ScheduleService)
- Endpoints: 7 (SchedulesController)

---

## ?? Documentation Files (Choose What You Need)

- **INDEX.md** - Navigation guide
- **README.md** - Project overview
- **QUICK_REFERENCE.md** - API commands
- **ARCHITECTURE_GUIDE.md** - Code structure
- **MIGRATION_GUIDE.md** - Database setup
- **COPILOT_GUIDE.md** - Using AI
- **API_TEST_EXAMPLES.md** - Test scenarios
- **FRONTEND_INTEGRATION.md** - Frontend guide

---

## ? Key Features

? **7 API Endpoints**
? **Full CRUD Operations**
? **Comprehensive Validation**
? **Error Handling**
? **Logging Integration**
? **DTO Pattern**
? **Clean Architecture**
? **Production Ready**

---

## ?? Common Tasks

### **Test API with Postman**
1. Open Postman
2. Create new request
3. Choose GET/POST/PUT/DELETE
4. Paste endpoint: `https://localhost:7xxx/api/schedules`
5. Click Send

### **Test with cURL**
```bash
curl -X GET "https://localhost:7xxx/api/schedules" \
  -H "Accept: application/json"
```

### **Test with Swagger UI**
1. Open: `https://localhost:7xxx/swagger/index.html`
2. Expand endpoint
3. Click "Try it out"
4. Enter data
5. Click "Execute"

### **Add Frontend Integration**
See: FRONTEND_INTEGRATION.md

---

## ?? Your Next Actions

```
1?? Setup (5 min)
   ? Update Connection String
   ? Run Migration
   ? Start Application

2?? Verify (5 min)
   ? Open Swagger UI
   ? Test GET endpoint
   ? Test POST endpoint

3?? Understand (15 min)
   ? Read QUICK_REFERENCE.md
   ? Read ARCHITECTURE_GUIDE.md
   ? Review ScheduleService.cs

4?? Integrate (30 min)
   ? Connect Frontend
   ? Test all endpoints
   ? Deploy!
```

---

## ?? Still Need Help?

See: **[INDEX.md](./INDEX.md)** for detailed navigation

---

## ?? You're Ready!

Everything is setup, configured, and ready to go.

**Just follow the 4 steps above and you're done!** ??

---

**Questions?** Check [INDEX.md](./INDEX.md) - it has answers to everything!

Happy coding! ??
