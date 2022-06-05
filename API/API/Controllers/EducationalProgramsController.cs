using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{

    [EnableCors("Cau Khong")]
    [Route("api/education-program")]
    [ApiController]
    public class EducationalProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EducationalProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EducationalPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EducationalProgram>>> GetEducationalProgram()
        {
          if (_context.EducationalProgram == null)
          {
              return NotFound();
          }
            return await _context.EducationalProgram.ToListAsync();
        }

        // GET: api/EducationalPrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EducationalProgram>> GetEducationalProgram(string id)
        {
          if (_context.EducationalProgram == null)
          {
              return NotFound();
          }
            var educationalProgram = await _context.EducationalProgram.FindAsync(id);

            if (educationalProgram == null)
            {
                return NotFound();
            }

            return educationalProgram;
        }

        [HttpGet("course/{id}")]

        public async Task<ActionResult<IEnumerable<EducationalProgram>>> GetAllCourseInEducationalProgram(string id)
        {
            IEnumerable<CourseEducationProgram> courseEducationProgrames = _context.CourseEducationProgram.Where(courseEdu => courseEdu.EducationalProgramId == id).ToList();
            List<Course> courses = new List<Course>();
            foreach (var courseEducation in courseEducationProgrames)
            {
                Course findingCourse = _context.Courses.Find(courseEducation.CourseId);
                courses.Add(findingCourse);
            }

            return Ok(courses);
        }
        // POST: api/EducationalPrograms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EducationalProgram>> PostEducationalProgram(CreateEducationalProgramDto request)
        {
          if (_context.EducationalProgram == null)
          {
              return Problem("Entity set 'ApplicationDbContext.EducationalProgram'  is null.");
          }
          var educationalProgram = new EducationalProgram()
          {
              EducationalProgramId = request.EducationalProgramId,
              Name = request.Name,
          };
            _context.EducationalProgram.Add(educationalProgram);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EducationalProgramExists(educationalProgram.EducationalProgramId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEducationalProgram", new { id = educationalProgram.EducationalProgramId }, educationalProgram);
        }

        // DELETE: api/EducationalPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducationalProgram(string id)
        {
            if (_context.EducationalProgram == null)
            {
                return NotFound();
            }
            var educationalProgram = await _context.EducationalProgram.FindAsync(id);
            if (educationalProgram == null)
            {
                return NotFound();
            }

            _context.EducationalProgram.Remove(educationalProgram);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //Add course to Educational Program
        [HttpPost("course")]
        public async Task<ActionResult<CourseEducationProgram>> AddCourse(AddCourseToEducationalProgramDto request)
        {
            Course course = _context.Courses.Find(request.CourseId);
            EducationalProgram educationalProgram = _context.EducationalProgram.Find(request.EducationalProgramId);
            if (course != null && educationalProgram != null)
            {
                var courseEducationalProgram = new CourseEducationProgram
                {
                    Course = course,
                    EducationalProgram = educationalProgram,
                    Semester = request.Semester,
                };
                _context.CourseEducationProgram.Add(courseEducationalProgram);
                _context.SaveChanges(); 
                return Ok(courseEducationalProgram);
            }

            return NotFound();
        }

        private bool EducationalProgramExists(string id)
        {
            return (_context.EducationalProgram?.Any(e => e.EducationalProgramId == id)).GetValueOrDefault();
        }
    }
}
