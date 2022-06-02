using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;
using API.Models.DatabaseModels;

namespace API.Controllers
{
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
        public async Task<ActionResult<IEnumerable<EducationalProgram>>> Get(int Id)
        {
            EducationalProgram educationalProgram = _context.EducationalProgram.Find(Id);
            return Ok(educationalProgram);
        }

        [HttpPost("{id}/{name}")]
        public async Task<ActionResult<EducationalProgram>> Create(int id, string name)
        {
            var educationalProgram = new EducationalProgram
            {
                Id = id,
                Name = name,
                CourseEducationalProgram = null
            };
            return Ok(Get(educationalProgram.Id));
        }
    }
}
