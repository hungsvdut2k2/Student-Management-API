using API.Data;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("Get/{Id}")]
        [HttpGet]
        public async Task<ActionResult<Course>> Get(int Id)
        {
            return await _context.Courses.FindAsync(Id);
        }
        [Route("GetCourseByEducationProgramId/{EducationalProgramId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetByEducationalProgram(int EducationalProgramId)
        {
            var CourseList = (from w in _context.CourseEducationalPrograms
                where w.EducationalProgramId == EducationalProgramId
                select w).ToList();
            List<Course> resCoursesList = new List<Course>();
            foreach (var course in CourseList)
            {
                Course findingCourse = await _context.Courses.FindAsync(course.CourseId);
                resCoursesList.Add(findingCourse);
            }

            return Ok(resCoursesList);
        }
        [Route("Delete/{Id}")]
        [HttpDelete]
        public async Task<ActionResult<Course>> Delete(int CourseId)
        {
            var course = await _context.Courses.FindAsync(CourseId);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Route("GetAllCourseClass/{CourseId}")]
        [HttpGet]
        public async Task<ActionResult<CourseClassroom>> GetAllCourseClass(int CourseId)
        {
            var classList = (from w in _context.CoursesClassroom
                where w.CourseId == CourseId
                select w).ToList();
            return Ok(classList);
        }
    }
}
