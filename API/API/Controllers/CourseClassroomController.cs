using API.Data;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseClassroomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseClassroomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetStudents")]
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetAllStudentsInClass(int ClassId)
        {
            var CourseList = (from w in _context.CourseClassroomUserInformations
                where w.CourseClassId == ClassId
                select w).ToList();
            List<UserInformation> resUserList = new List<UserInformation>();
            foreach (var course in CourseList)
            {
                UserInformation findingUser = await _context.UsersInformation.FindAsync(course.UserInformationId);
                resUserList.Add(findingUser);
            }
            return Ok(resUserList);
        }
        [HttpGet("GetClasses")]
        public async Task<ActionResult<IEnumerable<CourseClassroom>>> GetClasses(int CourseId)
        {
            var courseClassList = (from w in _context.CoursesClassroom
                where w.CourseId == CourseId
                select w).ToList();
            return Ok(courseClassList);
        }
        [HttpDelete]
        public async Task<ActionResult<CourseClassroom>> Delete(int Id)
        {
            CourseClassroom deletedClassroom = _context.CoursesClassroom.Where(w => w.CourseId == Id).FirstOrDefault();
            if (deletedClassroom == null)
            {
                return NotFound();
            }
            _context.CoursesClassroom.Remove(deletedClassroom);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CourseClassroomUserInformation>> Create(string UserInformationId, int CourseId)
        {
            CourseClassroomUserInformation courseUserInformation = new CourseClassroomUserInformation
            {
                UserInformationId = UserInformationId,
                UserInformation = null,
                CourseClassId = CourseId,
                CourseClassroom = null
            };
            _context.CourseClassroomUserInformations.Add(courseUserInformation);
            await _context.SaveChangesAsync();
            return Ok(courseUserInformation);
        }
    }
}
