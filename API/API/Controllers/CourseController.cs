using API.Data;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public async Task<ActionResult<Course>> Get(int Id)
        {
            return await _context.Courses.FindAsync(Id);
        }

        [HttpGet("GetCourseByEducationProgramId")]
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
    }
}
