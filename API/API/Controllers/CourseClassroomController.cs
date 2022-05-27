using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
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
        public async Task<ActionResult<IEnumerable<UserInformation>>> GetAllStudentsInClass(int classId)
        {
            var CourseList = (from w in _context.CourseClassroomUserInformations
                where w.CourseClassId == classId
                select w).ToList();
            List<UserInformation> resUserList = new List<UserInformation>();
            foreach (var course in CourseList)
            {
                UserInformation findingUser = await _context.UsersInformation.FindAsync(course.UserInformationId);
                resUserList.Add(findingUser);
            }
            return Ok(resUserList);
        }
        [HttpGet("classes/{courseId}")]
        public async Task<ActionResult<IEnumerable<CourseClassroom>>> GetClasses(int courseId)
        {
            var courseClassList = (from w in _context.CoursesClassroom
                where w.CourseId == courseId
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
        [Route("student/{UserInformationId}/{CourseClassId}")]
        [HttpPost]
        public async Task<ActionResult<CourseClassroomUserInformation>> Create(string UserInformationId, int CourseClassId)
        {
            UserInformation userInformation = await _context.UsersInformation.FindAsync(UserInformationId);
            CourseClassroom courseClass = await _context.CoursesClassroom.FindAsync(CourseClassId); 
            CourseClassroomUserInformation courseUserInformation = new CourseClassroomUserInformation
            {
                UserInformationId = UserInformationId,
                UserInformation = userInformation,
                CourseClassId = CourseClassId,
                CourseClassroom = courseClass
            };
            Score score = new Score
            {
                UserInformation = userInformation,
                UserInformationId = UserInformationId,
                CourseClassroom = courseClass,
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

        [HttpPost("course")]
        public async Task<ActionResult<List<CourseClassroom>>> AddCourseClassroom(CreateCourseClassroomDto request)
        {
            Course course = _context.Courses.Find(request.CourseId);
            CourseClassroom courseClassroom = new CourseClassroom
            {
                TeacherName = request.TeacherName,
                Course = course,
                Schedule = request.Schedule
            };
            _context.CoursesClassroom.Add(courseClassroom);
            _context.SaveChangesAsync();
            return Ok(courseClassroom);
        }
    }
}
