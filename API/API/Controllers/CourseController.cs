using API.Data;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("{Id}")]
        [HttpGet]
        public async Task<ActionResult<Course>> Get(int Id)
        {
            return await _context.Courses.FindAsync(Id);
        }
        [Route("educational-program/{educationalProgramId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetByEducationalProgram(int educationalProgramId)
        {
            var CourseList = (from w in _context.CourseEducationalPrograms
                where w.EducationalProgramId == educationalProgramId
                select w).ToList();
            List<Course> resCoursesList = new List<Course>();
            foreach (var course in CourseList)
            {
                Course findingCourse = await _context.Courses.FindAsync(course.CourseId);
                resCoursesList.Add(findingCourse);
            }

            return Ok(resCoursesList);
        }
        [Route("{courseId}")]
        [HttpDelete]
        public async Task<ActionResult<Course>> Delete(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Route("classes/{courseId}")]
        [HttpGet]
        public async Task<ActionResult<CourseClassroom>> GetAllCourseClass(int courseId)
        {
            var classList = (from w in _context.CoursesClassroom
                where w.CourseId == courseId
                select w).ToList();
            return Ok(classList);
        }
    }
}
