using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models.DtoModels;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/classroom-management")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClassroomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> Get()
        {
            return _context.Classrooms.ToList();
        }
        [Route("managed-classroom/{Id}")]
        [HttpGet]
        public async Task<ActionResult<Classroom>> GetClassroomById(int Id)
        {
            Classroom classroom = _context.Classrooms.Where(w => w.Id == Id).FirstOrDefault();
            return Ok(classroom);
        }
        [Route("managed-classroom/faculty/{Id}")]
        [HttpGet]
        public async Task<ActionResult<List<Classroom>>> GetClassroomByFacultyId(int Id)
        {
            var ClassList = (from w in _context.Classrooms
                                   where w.FacultyId == Id
                                   select w).ToList();
            return Ok(ClassList);
        }
        [HttpPost]
        public async Task<ActionResult<Classroom>> Post(CreateClassroomDto request)
        {
            Faculty faculty = _context.Faculty.Where(w => w.Id == request.FacultyId).FirstOrDefault();
            if (faculty == null)
            {
                return NotFound();
            }
            Classroom newClassroom = new Classroom
            {
                Name = request.nameOfClassroom,
                Faculty = faculty
            };
            _context.Classrooms.Add(newClassroom);
            await _context.SaveChangesAsync();
            return await GetClassroomById(newClassroom.Id);
        }
        [Route("{Id}")]
        [HttpDelete]
        public async Task<ActionResult<Classroom>> Delete(int Id)
        {
            var deletedClassroom =  _context.Classrooms.Where(w => w.Id == Id).FirstOrDefault();
            if(deletedClassroom == null)
            {
                return NotFound();
            }
            _context.Classrooms.Remove(deletedClassroom);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
