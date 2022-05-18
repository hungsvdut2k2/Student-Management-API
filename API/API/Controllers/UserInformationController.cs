using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<UserInformation>> Get(string Id)
        {
            UserInformation userInformation = await _context.UsersInformation.FindAsync(Id);
            Classroom classroom = await _context.Classrooms.FindAsync(userInformation.ClassroomId);
            Faculty faculty = await _context.Faculty.FindAsync(classroom.FacultyId);
            var returnedInformation = new
            {
                UserInformation = userInformation,
                Classroom = classroom,
                Faculty = faculty
            };
            return Ok(returnedInformation);
        }

        [HttpPut("Put")]
        public async Task<ActionResult<UserInformation>> Put(InformationDto request)
        {
            UserInformation newUserInformation =  await _context.UsersInformation.FindAsync(request.UserId);
            newUserInformation.Name = request.Name;
            newUserInformation.Dob = request.Dob;
            newUserInformation.Email = request.Email;
            newUserInformation.Gender = request.Gender;
            newUserInformation.PhoneNumber = request.PhoneNumber;
            await _context.SaveChangesAsync();
            return await Get(newUserInformation.UserId);
        }

        [HttpPost("Post")]
        public async Task<ActionResult<UserInformation>> Post(InformationDto request, int ClassId)
        {
            Classroom classroom = await _context.Classrooms.FindAsync(ClassId);
            if (classroom == null)
            {
                return NotFound();
            }
            UserInformation newUserInformation = new UserInformation
            {
                UserId = request.UserId,
                Name = request.Name,
                Dob = request.Dob,
                Email = request.Email,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Classroom = classroom,
                CourseClassroomUserInformation = null
            };
            await _context.UsersInformation.AddAsync(newUserInformation);
            await _context.SaveChangesAsync();
            return Ok(newUserInformation);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<UserInformation>> Delete(string Id)
        {
            UserInformation userInformation = await _context.UsersInformation.FindAsync(Id);
            if (userInformation == null)
            {
                return NotFound();
            }

            _context.UsersInformation.Remove(userInformation);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GetAllStudentsInClass")]
        public async Task<ActionResult<List<UserInformation>>> GetAllStudentInClass(int ClassroomId)
        {
            var studentList = (from w in _context.UsersInformation
                where w.ClassroomId == ClassroomId
                select w).ToList();
            return Ok(studentList);
        }

        [HttpGet("GetCourseClassroom")]
        public async Task<ActionResult<List<CourseClassroom>>> GetAllCourseClassroom(string UserInformationId)
        {
            var classList = (from w in _context.CourseClassroomUserInformations
                where w.UserInformationId == UserInformationId
                select w).ToList();
            var resCourseClassList = new List<CourseClassroom>();
            foreach (var courseClassroom in classList)
            {
                CourseClassroom findingClass = await _context.CoursesClassroom.FindAsync(courseClassroom.CourseClassId);
                resCourseClassList.Add(findingClass);
            }
            return Ok(resCourseClassList);
        }
    }
}
