# ?? Database Migration Guide - Schedule Schema Update

## ?? Tóm T?t Thay ??i Schema

Database schema cho `Schedules` table ?ã ???c c?p nh?t:

### **Tr??c (Old Schema):**
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

### **Sau (New Schema):**
```sql
CREATE TABLE Schedules (
    ScheduleID INT PRIMARY KEY IDENTITY(1,1),
    ClassID INT NOT NULL,
    Room NVARCHAR(100),
    ClassDate DATETIME NOT NULL,        -- Thay th? StartDate + EndDate + DayOfWeek
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedAt DATETIME DEFAULT GETDATE(),    -- Rename: CreatedDate ? CreatedAt
    UpdatedAt DATETIME DEFAULT GETDATE(),     -- Rename: UpdatedDate ? UpdatedAt (nullable)
    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID) ON DELETE CASCADE
);
```

### **Thay ??i Chi Ti?t:**

| Tr??ng | Thay ??i | Lý Do |
|--------|----------|-------|
| `StartDate` | ? Xóa | Không c?n separate start/end dates, m?t l?p h?c trên m?t l?p h?c |
| `EndDate` | ? Xóa | Gi?ng nh? trên |
| `DayOfWeek` | ? Xóa | `ClassDate` ?ã ch?a ngày trong tu?n |
| `ClassDate` | ? Thêm m?i | Ngày h?c c? th? (YYYY-MM-DD) |
| `CreatedDate` | ? Rename ? `CreatedAt` | Naming convention consistency |
| `UpdatedDate` | ? Rename ? `UpdatedAt` (nullable) | Naming convention + nullable khi ch?a c?p nh?t |

---

## ?? Cách Th?c Hi?n Migration

### **B??c 1: Backup Database (Quan Tr?ng!)**

Tr??c khi migration, hãy backup database c?a b?n:

```powershell
# S? d?ng SQL Server Management Studio (SSMS)
# 1. Right-click trên database "EduCenter_CourseDB"
# 2. Tasks > Backup...
# 3. Ch?n location và backup
```

### **B??c 2: T?o Migration M?i**

M? **Package Manager Console** trong Visual Studio:

```powershell
# ?i t?i th? m?c project ?úng
cd CourseAndScheduleService\CourseAndScheduleService

# T?o migration
Add-Migration UpdateScheduleSchema

# Ho?c n?u mu?n tên c? th? h?n:
Add-Migration ScheduleSchemaUpdate_RemoveDateRangeAddClassDate
```

**K?t qu?:**
M?t file migration m?i s? ???c t?o trong folder `Migrations/`, ví d?:
```
Migrations/
??? 20240115_InitialCreate.cs
??? 20240115_UpdateScheduleSchema.cs  ? File m?i t?o
```

### **B??c 3: Ki?m Tra Migration File**

File migration t? ??ng generate s? trông nh? th? này:

```csharp
public partial class UpdateScheduleSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Xóa constraints c?
        migrationBuilder.DropColumn(
            name: "StartDate",
            table: "Schedules");

        migrationBuilder.DropColumn(
            name: "EndDate",
            table: "Schedules");

        migrationBuilder.DropColumn(
            name: "DayOfWeek",
            table: "Schedules");

        migrationBuilder.RenameColumn(
            name: "CreatedDate",
            table: "Schedules",
            newName: "CreatedAt");

        migrationBuilder.RenameColumn(
            name: "UpdatedDate",
            table: "Schedules",
            newName: "UpdatedAt");

        // Thêm column m?i
        migrationBuilder.AddColumn<DateTime>(
            name: "ClassDate",
            table: "Schedules",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert migration n?u c?n
        // ...
    }
}
```

**?? N?u mu?n custom migration:**

B?n có th? edit file này tr??c khi apply. Ví d?: thêm data migration:

```csharp
public partial class UpdateScheduleSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // ... existing column changes ...

        // Migrate d? li?u c? sang m?i
        migrationBuilder.Sql(@"
            UPDATE Schedules
            SET ClassDate = StartDate
            WHERE ClassDate IS NULL;
        ");
    }
}
```

### **B??c 4: Apply Migration**

Sau khi ki?m tra, apply migration:

```powershell
Update-Database
```

**Ho?c ?? apply migration c? th?:**

```powershell
Update-Database -Migration UpdateScheduleSchema
```

**Output k? v?ng:**
```
Done. Applying migration '20240115_UpdateScheduleSchema.cs'.
```

### **B??c 5: Verify Changes trong SQL Server**

M? **SQL Server Management Studio** và ch?y:

```sql
USE EduCenter_CourseDB;
GO

-- Xem structure table
EXEC sp_help 'Schedules';

-- Ho?c view chi ti?t columns
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Schedules'
ORDER BY ORDINAL_POSITION;
```

**Expected Output:**
```
ScheduleID       INT            NO
ClassID          INT            NO
Room             NVARCHAR(100)  YES
ClassDate        DATETIME       NO
StartTime        TIME           NO
EndTime          TIME           NO
Status           NVARCHAR(50)   YES
CreatedAt        DATETIME       NO
UpdatedAt        DATETIME       YES
```

---

## ?? N?u Có D? Li?u C?

N?u database ?ã có d? li?u l?u v?i schema c?:

### **Option 1: Migrate D? Li?u (Recommended)**

```sql
-- Backup d? li?u c?
SELECT * INTO Schedules_Backup FROM Schedules;

-- Migrate t? StartDate + DayOfWeek sang ClassDate
-- (S? d?ng StartDate làm ClassDate - ngày b?t ??u l?p h?c)
UPDATE Schedules
SET ClassDate = StartDate;

-- Xóa columns c?
ALTER TABLE Schedules DROP COLUMN StartDate;
ALTER TABLE Schedules DROP COLUMN EndDate;
ALTER TABLE Schedules DROP COLUMN DayOfWeek;
```

### **Option 2: Reset Database (N?u d? li?u không quan tr?ng)**

```powershell
# Remove all migrations
Update-Database -Migration 0

# Xóa folder Migrations
# (Ho?c xóa file migration m?i nh?t)

# T?o migration t? ??u
Add-Migration InitialCreate

# Apply
Update-Database
```

---

## ?? Rollback Migration (N?u C?n)

N?u mu?n quay l?i version c?:

```powershell
# Quay l?i migration tr??c ?ó
Update-Database -Migration <PreviousMigrationName>

# Ví d?: quay l?i migration InitialCreate
Update-Database -Migration InitialCreate

# Ho?c quay l?i tr?ng thái tr??c migration ??u tiên
Update-Database -Migration 0
```

---

## ?? Test API Sau Migration

Sau khi migration thành công, test các endpoint:

### **1. Get All Schedules**
```bash
GET /api/schedules
```

**Expected Response:**
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
      "updatedAt": null
    }
  ]
}
```

### **2. Create New Schedule**
```bash
POST /api/schedules
{
  "classID": 1,
  "room": "A101",
  "classDate": "2024-02-20T00:00:00",
  "startTime": "08:00:00",
  "endTime": "10:00:00",
  "status": "Active"
}
```

### **3. Update Schedule**
```bash
PUT /api/schedules/1
{
  "scheduleID": 1,
  "classID": 1,
  "room": "A102",
  "classDate": "2024-02-20T00:00:00",
  "startTime": "09:00:00",
  "endTime": "11:00:00",
  "status": "Active"
}
```

### **4. Get Schedule by Date**
```bash
GET /api/schedules/date/2024-01-15
```

---

## ?? Troubleshooting Migration

### **Error: "Cannot add NOT NULL column"**

```
L?i: Cannot insert the value NULL into column 'ClassDate'
```

**Gi?i pháp:**

```powershell
# Quay l?i
Update-Database -Migration <PreviousMigration>

# Edit migration file và thêm default value
migrationBuilder.AddColumn<DateTime>(
    name: "ClassDate",
    table: "Schedules",
    type: "datetime2",
    nullable: false,
    defaultValue: new DateTime(2024, 1, 1));

# Th? l?i
Update-Database
```

---

### **Error: "A migration with name 'X' already exists"**

```
L?i: A migration with name 'UpdateScheduleSchema' already exists in project
```

**Gi?i pháp:**

```powershell
# Xóa migration file v?a t?o (n?u ch?a apply)
Remove-Migration

# T?o l?i v?i tên khác
Add-Migration ScheduleSchemaUpdate_V2
```

---

### **Error: "Enlist in local transaction failed"**

**Gi?i pháp:**

```powershell
# Close t?t c? connection t?i database
# ??m b?o SSMS không ?ang k?t n?i

# Th? l?i
Update-Database
```

---

## ?? Tham Kh?o Entity Framework Core Migrations

```powershell
# List t?t c? migrations
Get-Migration

# Xem migration file chi ti?t
Get-Migration -Detailed

# Ki?m tra migration nào ?ang apply
Add-Migration -Verbose

# Script migration (không apply, ch? xem SQL)
Script-Migration -From <From> -To <To>
```

---

## ? Checklist Migration

- [ ] Backup database
- [ ] T?o migration: `Add-Migration UpdateScheduleSchema`
- [ ] Ki?m tra migration file
- [ ] Apply migration: `Update-Database`
- [ ] Verify trong SSMS
- [ ] Test API endpoints
- [ ] Check logs trong Visual Studio Output window
- [ ] Confirm d? li?u migrate thành công (n?u có)

---

**Migration Complete! ?**

C?u trúc database gi? ?ây ??n gi?n, rõ ràng và d? b?o trì h?n.
