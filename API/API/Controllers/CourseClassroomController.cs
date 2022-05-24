using API.Data;
using API.Models.DatabaseModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/course-class")]
    [ApiController]
    public class CourseClassroomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseClassroomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("students/{classId}")]
        [HttpGet]
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
        [Route("classes/{courseId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseClassroom>>> GetClasses(int CourseId)
        {
            var courseClassList = (from w in _context.CoursesClassroom
                where w.CourseId == CourseId
                select w).ToList();
            return Ok(courseClassList);
        }
        [Route("{Id}")]
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
        [Route("{UserInformationId}/{CourseClassId}")]
        [HttpPost]
        public async Task<ActionResult<CourseClassroomUserInformation>> Create(string UserInformationId, int CourseClassId)
        {
            CourseClassroomUserInformation courseUserInformation = new CourseClassroomUserInformation
            {
                UserInformationId = UserInformationId,
                UserInformation = null,
                CourseClassId = CourseClassId,
                CourseClassroom = null
            };
            Score score = new Score
            {
                UserInformation = null,
                UserInformationId = UserInformationId,
                CourseClassroom = null,
                CourseClassroomId = CourseClassId,
                ExcerciseRate = 0.2,
                MidTermRate = 0.3,
                FinalTermRate = 0.5,
                ExcerciseScore = 0,
                MidTermScore = 0,
                FinalTermScore = 0
            };
            _context.Score.Add(score);
            _context.CourseClassroomUserInformations.Add(courseUserInformation);
            await _context.SaveChangesAsync();
            return Ok(courseUserInformation);
        }
    }
}
