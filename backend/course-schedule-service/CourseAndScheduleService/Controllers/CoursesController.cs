using CourseAndScheduleService.Data;
using CourseAndScheduleService.Models;
using CourseAndScheduleService.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAndScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EduCenterDbContext _context;

        public CoursesController(EduCenterDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Course>>>> GetAllCourses()
        {
            try
            {
                var courses = await _context.Courses.Include(c => c.Classes).ToListAsync();
                return Ok(ApiResponse<List<Course>>.SuccessResponse("L?y danh sách khóa h?c thành công", courses));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<Course>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Course>>> GetCourseById(int id)
        {
            try
            {
                var course = await _context.Courses
                    .Include(c => c.Classes)
                    .FirstOrDefaultAsync(c => c.CourseID == id);

                if (course == null)
                {
                    return NotFound(ApiResponse<Course>.ErrorResponse("Khóa h?c không t?n t?i"));
                }

                return Ok(ApiResponse<Course>.SuccessResponse("L?y thông tin khóa h?c thành công", course));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Course>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Course>>> CreateCourse([FromBody] Course course)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<Course>.ErrorResponse("D? li?u không h?p l?"));
                }

                course.CreatedDate = DateTime.Now;
                course.UpdatedDate = DateTime.Now;

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseID }, 
                    ApiResponse<Course>.SuccessResponse("T?o khóa h?c thành công", course));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Course>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // PUT: api/courses/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Course>>> UpdateCourse(int id, [FromBody] Course course)
        {
            try
            {
                if (id != course.CourseID)
                {
                    return BadRequest(ApiResponse<Course>.ErrorResponse("ID không kh?p"));
                }

                var existingCourse = await _context.Courses.FindAsync(id);
                if (existingCourse == null)
                {
                    return NotFound(ApiResponse<Course>.ErrorResponse("Khóa h?c không t?n t?i"));
                }

                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.Credits = course.Credits;
                existingCourse.Status = course.Status;
                existingCourse.UpdatedDate = DateTime.Now;

                _context.Courses.Update(existingCourse);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<Course>.SuccessResponse("C?p nh?t khóa h?c thành công", existingCourse));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<Course>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        // DELETE: api/courses/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCourse(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Khóa h?c không t?n t?i"));
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse("Xóa khóa h?c thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }
    }
}
