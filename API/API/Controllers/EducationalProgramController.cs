using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/educational-program")]
    [ApiController]
    public class EducationalProgramController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EducationalProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EducationalProgram>>> GetAll()
        {
            List<EducationalProgram> educationalPrograms = _context.EducationalProgram.ToList();
            return Ok(educationalPrograms);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<EducationalProgram>> Get(int Id)
        {
            EducationalProgram educationalProgram = _context.EducationalProgram.Find(Id);
            return Ok(educationalProgram);
        }

        [HttpPost("{name}")]
        public async Task<ActionResult<EducationalProgram>> Create(string name)
        {
            var educationalProgram = new EducationalProgram
            {
                Name = name,
                CourseEducationalProgram = null
            };
            _context.Add(educationalProgram);
            _context.SaveChanges();
            return Ok((educationalProgram));
        }
    }
}
