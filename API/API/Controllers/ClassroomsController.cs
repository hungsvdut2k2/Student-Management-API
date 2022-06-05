using System;
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
    [Route("api/classroom")]
    [ApiController]
    public class ClassroomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassroomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Classrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassroom()
        {
          if (_context.Classroom == null)
          {
              return NotFound();
          }
            return await _context.Classroom.ToListAsync();
        }

        // GET: api/Classrooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(string id)
        {
          if (_context.Classroom == null)
          {
              return NotFound();
          }
            var classroom = await _context.Classroom.FindAsync(id);

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }


        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom(CreateClassroomDto request)
        {
          if (_context.Classroom == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Classroom'  is null.");
          }
          Faculty faculty = _context.Faculty.Where(w => w.FacultyId == request.FacultyId).FirstOrDefault();
          EducationalProgram educationalProgram = _context.EducationalProgram.Where(w => w.EducationalProgramId == request.EducationalProgramId).First();
          int AcademicYear = Convert.ToInt32(request.Name.Substring(0, 2));
          int numberOfClasses = (_context.Classroom.Where(w => w.FacultyId == faculty.FacultyId).ToList()).Count() + 1;
          string classId;
          if (numberOfClasses < 10)
          {
              classId = "0" + Convert.ToString(numberOfClasses);
          }
          else
          {
              classId = Convert.ToString(numberOfClasses);
          }
          if (faculty == null)
          {
              return NotFound();
          }
          Classroom classroom = new Classroom
          {
              ClassroomId = faculty.FacultyId + Convert.ToString(AcademicYear) + classId,
              Name = request.Name,
              Faculty = faculty,
              AcademicYear = AcademicYear,
              EducationalProgram = educationalProgram
          };
            _context.Classroom.Add(classroom);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClassroomExists(classroom.ClassroomId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClassroom", new { id = classroom.ClassroomId }, classroom);
        }

        // DELETE: api/Classrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(string id)
        {
            if (_context.Classroom == null)
            {
                return NotFound();
            }
            var classroom = await _context.Classroom.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            _context.Classroom.Remove(classroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassroomExists(string id)
        {
            return (_context.Classroom?.Any(e => e.ClassroomId == id)).GetValueOrDefault();
        }
    }
}
