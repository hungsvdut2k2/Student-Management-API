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
    [Route("api/faculty")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FacultiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Faculties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculty()
        {
          if (_context.Faculty == null)
          {
              return NotFound();
          }
            return await _context.Faculty.ToListAsync();
        }

        // GET: api/Faculties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> GetFaculty(string id)
        {
          if (_context.Faculty == null)
          {
              return NotFound();
          }
            var faculty = await _context.Faculty.FindAsync(id);

            if (faculty == null)
            {
                return NotFound();
            }

            return faculty;
        }

        [HttpGet("classes/{facultyId}")]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetAllClassesInFaculty(string facultyId)
        {
            List<Classroom> classrooms =
                _context.Classroom.Where(classroom => classroom.FacultyId == facultyId).ToList();
            return Ok(classrooms);
        }


        // POST: api/Faculties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Faculty>> PostFaculty(CreateFacultyDto request)
        {
          if (_context.Faculty == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Faculty'  is null.");
          }

          var faculty = new Faculty
          {
              FacultyId = request.FacultyId,
              Name = request.Name,
          };
            _context.Faculty.Add(faculty);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FacultyExists(faculty.FacultyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFaculty", new { id = faculty.FacultyId }, faculty);
        }

        // DELETE: api/Faculties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            if (_context.Faculty == null)
            {
                return NotFound();
            }
            var faculty = await _context.Faculty.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            _context.Faculty.Remove(faculty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacultyExists(string id)
        {
            return (_context.Faculty?.Any(e => e.FacultyId == id)).GetValueOrDefault();
        }
    }
}
