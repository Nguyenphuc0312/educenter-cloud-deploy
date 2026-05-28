# ?? GitHub Copilot Integration Guide

## ?? M?c ?ích

File này giúp b?n s? d?ng **GitHub Copilot Chat** trong Visual Studio ??:
1. Hi?u c?u trúc project hi?n t?i
2. Vi?t code theo pattern ?ã có
3. Thêm features m?i m?t cách consistent
4. T?o unit tests

---

## ?? Cách S? D?ng Copilot Hi?u Qu?

### **B??c 1: M? File "Pattern"**

Tr??c khi yêu c?u Copilot, hãy m? m?t file có c?u trúc t??ng t?:

? **Ví d? 1: Mu?n thêm CRUD m?i**
- M? `Services/Implementations/ScheduleService.cs`
- Copilot s? "b?t ch??c" style này

? **Ví d? 2: Mu?n thêm Controller**
- M? `Controllers/SchedulesController.cs`
- Copilot s? vi?t controller t??ng t?

? **Ví d? 3: Mu?n thêm DTO**
- M? `DTOs/ScheduleDto.cs`
- Copilot s? t?o DTOs theo format này

---

## ?? Prompt Examples For Copilot

### **Example 1: T?o Service M?i (CourseService)**

```
@workspace D?a trên pattern c?a ScheduleService, hãy t?o CourseService 
v?i các method:
- GetAllCoursesAsync()
- GetCourseByIdAsync(id)
- CreateCourseAsync(dto)
- UpdateCourseAsync(dto)
- DeleteCourseAsync(id)

Các validation:
- Credits ph?i > 0
- CourseName không ???c tr?ng
- CourseName ?? dài 1-100 ký t?

Hãy tuân theo c?u trúc c?a ScheduleService:
1. T?o interface IScheduleService
2. T?o service implementation v?i logging
3. Thêm validation method riêng
4. S? d?ng try-catch & ArgumentException

Output: Toàn b? code t?ng file m?t
```

### **Example 2: Thêm DTOs M?i**

```
@workspace Xem file DTOs/ScheduleDto.cs, sau ?ó t?o CourseDto.cs v?i:
- CourseDtoRead (?? tr? v? API)
- CreateCourseDto (?? POST)
- UpdateCourseDto (?? PUT)

Tuân theo naming convention và structure c?a ScheduleDto.cs
```

### **Example 3: Thêm Controller**

```
@workspace D?a trên SchedulesController.cs, t?o CoursesController v?i:
- Inject IScheduleService thay vì IScheduleService
- 7 endpoints: GET (all), GET (by id), POST, PUT, DELETE
- T?t c? return ApiResponse<T> format

Gi? nguyên error handling pattern, logging, validation
```

### **Example 4: Unit Test**

```
@workspace Vi?t unit test cho ScheduleService.CreateScheduleAsync():

1. Test case thành công: valid data ? return ScheduleDto
2. Test case fail: invalid time ? throw ArgumentException
3. Test case fail: class not exist ? throw ArgumentException

Dùng Moq framework, xUnit
```

### **Example 5: Fix Issues**

```
@workspace Tôi g?p l?i [error message]. 
Xem c?u trúc project và tìm l?i t??ng t? trong codebase, 
sau ?ó gi?i pháp fix.
```

---

## ?? Best Practices For Prompting

### ? DO: Specific & Detailed
```
"D?a trên ScheduleService pattern, t?o CourseService v?i 
GetAllCoursesAsync() method, include logging, validation, 
và MapToDto() helper. Tuân theo async/await pattern."
```

### ? DON'T: Too Vague
```
"T?o course service"
```

---

### ? DO: Reference Existing Code
```
"Xem file Services/Implementations/ScheduleService.cs, 
sau ?ó t?o file t??ng t? cho Teacher entity"
```

### ? DON'T: No Context
```
"Vi?t service logic"
```

---

### ? DO: Specify Requirements
```
"T?o DTO v?i các field: Id, Name, Email. 
Exclude CreatedAt/UpdatedAt t? CreateDto. 
UpdateDto c?n có Id."
```

### ? DON'T: Let Copilot Guess
```
"T?o DTO"
```

---

## ?? Copilot Commands You'll Use

### **File Context**
```
@workspace     - Copilot ??c c? project
@FileName      - Reference file c? th?
#reference     - Liên k?t t?i file khác
```

### **Asking For**
```
"T?o..."          - Generate code
"Vi?t..."         - Write code
"Xem l?i..."      - Review code
"Gi?i thích..."   - Explain code
"Fix l?i..."      - Fix bugs
"Refactor..."     - Improve code
"T?i ?u..."       - Optimize code
```

---

## ?? Common Tasks

### **Task 1: Thêm CRUD M?i Cho Entity**

**Files c?n t?o:**
```
1. DTOs/TeacherDto.cs
2. Services/Interfaces/ITeacherService.cs
3. Services/Implementations/TeacherService.cs
4. Controllers/TeachersController.cs
```

**Copilot Prompt:**
```
@workspace Tôi mu?n thêm CRUD cho Teacher entity.
Xem ScheduleService làm pattern, t?o:

1. DTOs/TeacherDto.cs (TeacherDto, CreateTeacherDto, UpdateTeacherDto)
2. ITeacherService interface v?i 7 methods
3. TeacherService implementation
4. TeachersController v?i 7 endpoints

Validation:
- Email ph?i unique & valid email format
- Name không ???c tr?ng (1-100 chars)
- Phone optional (10 digits n?u có)

Response format: ApiResponse<T> nh? hi?n t?i
```

---

### **Task 2: Fix Bug**

**Copilot Prompt:**
```
@workspace Tôi g?p l?i:
"Cannot insert NULL into column ClassID"

L?i này t? CreateScheduleAsync() method.
Xem code & tìm ra nguyên nhân + fix.
```

---

### **Task 3: Add Logging Everywhere**

**Copilot Prompt:**
```
@workspace Xem ScheduleService.cs, ?ó là pattern logging t?t.
Thêm logging t??ng t? vào CourseService:
- Info log khi method b?t ??u
- Warning log khi không tìm th?y resource
- Error log khi exception

Dùng ILogger<CourseService> dependency injection.
```

---

### **Task 4: Review Code Quality**

**Copilot Prompt:**
```
@workspace Code review file Controllers/SchedulesController.cs:
1. Có follow SOLID principles không?
2. Error handling ?? không?
3. Logging ?? không?
4. Có security issues không?

Suggest improvements.
```

---

## ?? Pro Tips

### **Tip 1: Use File Reference**
```
"Gi?ng nh? @ScheduleService.cs, t?o CourseService.cs"
? Copilot s? t?o code y chang structure
```

### **Tip 2: Incremental Requests**
```
1?? T?o DTO
2?? T?o Interface
3?? T?o Implementation
4?? T?o Controller

? Tách nh? requests ? Copilot hi?u t?t h?n
```

### **Tip 3: Copy Error Message**
```
Khi g?p l?i, copy toàn b? error message vào prompt
? Copilot d? debug h?n
```

### **Tip 4: Ask For Explanation**
```
"Gi?i thích dòng này: await _context.SaveChangesAsync()"
? Hi?u code t?t h?n
```

---

## ?? Template Prompts

### **Template 1: Add New Feature**
```
@workspace

Feature: [Feature Name]

Based on ScheduleService pattern, create:

1. Entity/Model: [Name]
2. DTOs: [List fields]
3. Service Interface: [List methods]
4. Service Implementation: [List methods]
5. Controller: [List endpoints]

Validations:
- [Validation 1]
- [Validation 2]

Response format: ApiResponse<T>
Error handling: Try-catch + ArgumentException
Logging: ILogger<[Service]>
```

### **Template 2: Code Review**
```
@workspace

Review this code for:
1. SOLID principles compliance
2. Error handling
3. Logging adequacy
4. Security issues
5. Performance concerns

File: [FileName]

Suggest improvements with code examples.
```

### **Template 3: Bug Fix**
```
@workspace

Error: [Error Message]
File: [FileName]
Method: [MethodName]

1. Analyze the root cause
2. Suggest fix
3. Provide fixed code
```

---

## ?? Expected Outcomes

### **After Copilot Generates Code:**

? **You should:**
1. Review the generated code
2. Understand what it does
3. Test it locally
4. Make adjustments if needed
5. Commit to git

? **Don't:**
1. Copy-paste without understanding
2. Deploy without testing
3. Ignore warnings/suggestions
4. Modify without understanding consequences

---

## ?? Copilot Accuracy Tips

| Scenario | Accuracy | Fix |
|----------|----------|-----|
| Simple CRUD | 95% | Usually works as-is |
| Complex Logic | 70% | Needs review & adjustment |
| Bug Fix | 60% | Provide more context |
| Unit Tests | 85% | Usually good, verify coverage |
| Performance | 50% | Requires manual optimization |

---

## ?? Productivity Hacks

### **Hack 1: Template Methods**
```
Ask Copilot: "T?o method CreateScheduleAsync() 
template v?i try-catch, validation, logging pattern"

? Copy template & reuse
```

### **Hack 2: Batch Generation**
```
"T?o 3 methods: GetAll, GetById, Delete
v?i same error handling pattern"

? Copilot t?o c? 3 cùng lúc
```

### **Hack 3: Explanation Request**
```
"Gi?i thích c?u trúc file này & t? ??ng t?o 
t??ng t? cho entity khác"

? Copilot hi?u pattern ? generate consistent
```

---

## ?? Common Copilot Mistakes

| Mistake | How to Avoid |
|---------|------------|
| Wrong async/await | Specify "use async/await" |
| Missing null checks | Mention "safe navigation" |
| No error handling | Say "add try-catch" |
| Wrong naming | Provide "naming convention" example |
| Missing logging | Reference ScheduleService |

---

## ?? Learning Path

1. **Week 1: Get Familiar**
   - Use Copilot for simple tasks
   - Review generated code
   - Understand patterns

2. **Week 2: Intermediate**
   - Add DTOs for existing entities
   - Create Services
   - Build Controllers

3. **Week 3: Advanced**
   - Complex business logic
   - Unit tests
   - Performance optimization

4. **Week 4: Mastery**
   - Lead refactoring
   - Mentor others
   - Use Copilot for code reviews

---

## ?? Final Tips

1. **Always Review**: Never deploy Copilot code without review
2. **Test Thoroughly**: Generated code needs testing
3. **Ask for Explanation**: Understand what Copilot generates
4. **Use as Helper**: Copilot is helper, not replacement
5. **Provide Context**: Better context ? Better code

---

## ?? You're Ready!

Gi? b?n ?ã có:
- ? Project structure hi?u rõ
- ? Pattern ?? Copilot follow
- ? Templates ?? reuse
- ? Guidelines ?? maintain quality

**Use Copilot wisely & efficiently!** ??

---

**Happy AI-Assisted Coding! ??**
