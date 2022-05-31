using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models.DtoModels;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/classroom")]
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
        [Route("{Id}")]
        [HttpGet]
        public async Task<ActionResult<Classroom>> GetClassroomById(string Id)
        {
            Classroom classroom = _context.Classrooms.Where(w => w.ClassroomId == Id).FirstOrDefault();
            return Ok(classroom);
        }
        [Route("faculty/{Id}")]
        [HttpGet]
        public async Task<ActionResult<List<Classroom>>> GetClassroomByFacultyId(string Id)
        {
            var ClassList = (from w in _context.Classrooms
                                   where w.FacultyId == Id
                                   select w).ToList();
            return Ok(ClassList);
        }
        [HttpPost]
        public async Task<ActionResult<Classroom>> Post(CreateClassroomDto request)
        {
            Faculty faculty = _context.Faculty.Where(w => w.FacultyId == request.FacultyId).FirstOrDefault();
            EducationalProgram educationalProgram = _context.EducationalProgram.Where(w => w.Id == request.EducationalProgramId).First();
            int AcademicYear = Convert.ToInt32(request.NameOfClassroom.Substring(0, 2));
            int numberOfClasses = (_context.Classrooms.Where(w => w.FacultyId == faculty.FacultyId).ToList()).Count() + 1;
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
            Classroom newClassroom = new Classroom
            {
                ClassroomId = faculty.FacultyId + Convert.ToString(AcademicYear) + classId,
                Name = request.NameOfClassroom,
                Faculty = faculty,
                AcademicYear = AcademicYear,
                EducationalProgram = educationalProgram
            };
            _context.Classrooms.Add(newClassroom);
            await _context.SaveChangesAsync();
            return await GetClassroomById(newClassroom.ClassroomId);
        }
        [Route("{Id}")]
        [HttpDelete]
        public async Task<ActionResult<Classroom>> Delete(string Id)
        {
            var deletedClassroom =  _context.Classrooms.Where(w => w.ClassroomId == Id).FirstOrDefault();
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
