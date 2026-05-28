using CourseAndScheduleService.Data;
using CourseAndScheduleService.Models;
using CourseAndScheduleService.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAndScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly EduCenterDbContext _context;

        public ClassesController(EduCenterDbContext context)
        {
            _context = context;
        }

        // GET: api/classes
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Class>>>> GetAllClasses()
        {
            try
            {
                var classes = await _context.Classes
                    .Include(c => c.Course)
                    .Include(c => c.Schedules)
                    .ToListAsync();
                return Ok(ApiResponse<List<Class>>.SuccessResponse("L?y danh sách l?p h?c thành công", classes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<Class>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // GET: api/classes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Class>>> GetClassById(int id)
        {
            try
            {
                var classData = await _context.Classes
                    .Include(c => c.Course)
                    .Include(c => c.Schedules)
                    .FirstOrDefaultAsync(c => c.ClassID == id);

                if (classData == null)
                {
                    return NotFound(ApiResponse<Class>.ErrorResponse("L?p h?c không t?n t?i"));
                }

                return Ok(ApiResponse<Class>.SuccessResponse("L?y thông tin l?p h?c thành công", classData));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Class>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // GET: api/classes/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<ApiResponse<List<Class>>>> GetClassesByCourse(int courseId)
        {
            try
            {
                var classes = await _context.Classes
                    .Where(c => c.CourseID == courseId)
                    .Include(c => c.Course)
                    .Include(c => c.Schedules)
                    .ToListAsync();

                return Ok(ApiResponse<List<Class>>.SuccessResponse("L?y danh sách l?p theo khóa h?c thành công", classes));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<Class>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // POST: api/classes
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Class>>> CreateClass([FromBody] Class classData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<Class>.ErrorResponse("D? li?u không h?p l?"));
                }

                // Ki?m tra khóa h?c có t?n t?i không
                var course = await _context.Courses.FindAsync(classData.CourseID);
                if (course == null)
                {
                    return BadRequest(ApiResponse<Class>.ErrorResponse("Khóa h?c không t?n t?i"));
                }

                classData.CreatedDate = DateTime.Now;
                classData.UpdatedDate = DateTime.Now;

                _context.Classes.Add(classData);
                await _context.SaveChangesAsync();

                // Reload ?? l?y Course navigation property
                await _context.Entry(classData).Reference(c => c.Course).LoadAsync();

                return CreatedAtAction(nameof(GetClassById), new { id = classData.ClassID }, 
                    ApiResponse<Class>.SuccessResponse("T?o l?p h?c thành công", classData));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Class>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // PUT: api/classes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Class>>> UpdateClass(int id, [FromBody] Class classData)
        {
            try
            {
                if (id != classData.ClassID)
                {
                    return BadRequest(ApiResponse<Class>.ErrorResponse("ID không kh?p"));
                }

                var existingClass = await _context.Classes.FindAsync(id);
                if (existingClass == null)
                {
                    return NotFound(ApiResponse<Class>.ErrorResponse("L?p h?c không t?n t?i"));
                }

                // Ki?m tra khóa h?c có t?n t?i không
                var course = await _context.Courses.FindAsync(classData.CourseID);
                if (course == null)
                {
                    return BadRequest(ApiResponse<Class>.ErrorResponse("Khóa h?c không t?n t?i"));
                }

                existingClass.ClassCode = classData.ClassCode;
                existingClass.CourseID = classData.CourseID;
                existingClass.Capacity = classData.Capacity;
                existingClass.EnrolledStudents = classData.EnrolledStudents;
                existingClass.Instructor = classData.Instructor;
                existingClass.Status = classData.Status;
                existingClass.UpdatedDate = DateTime.Now;

                _context.Classes.Update(existingClass);
                await _context.SaveChangesAsync();

                // Reload ?? l?y Course navigation property
                await _context.Entry(existingClass).Reference(c => c.Course).LoadAsync();

                return Ok(ApiResponse<Class>.SuccessResponse("C?p nh?t l?p h?c thành công", existingClass));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Class>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // DELETE: api/classes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteClass(int id)
        {
            try
            {
                var classData = await _context.Classes.FindAsync(id);
                if (classData == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("L?p h?c không t?n t?i"));
                }

                _context.Classes.Remove(classData);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse("Xóa l?p h?c thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }
    }
}
