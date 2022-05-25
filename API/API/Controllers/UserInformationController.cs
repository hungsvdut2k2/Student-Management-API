using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/user")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserInformationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("{Id}")]
        [HttpGet]
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

        [HttpPut]
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
        [Route("{ClassId}")]
        [HttpPost]
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
        [Route("{Id}")]
        [HttpDelete]
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
        [HttpGet("class/{classroomId}")]
        public async Task<ActionResult<List<UserInformation>>> GetAllStudentInClass(int classroomId)
        {
            var studentList = _context.UsersInformation.Where(w => w.ClassroomId == classroomId).ToList();
            return Ok(studentList);
        }
        [Route("course-class/{UserInformationId}")]
        [HttpGet]
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
       [Route("faculty/{facultyId}")]

        [HttpGet]
        public async Task<ActionResult<List<Classroom>>> GetAllStudentInFaculty(int facultyId)
        {
            Faculty faculty = await _context.Faculty.FindAsync(facultyId);
            List<Classroom> classList = _context.Classrooms.Where(w => w.FacultyId == faculty.Id).ToList();
            foreach (var Class in classList)
            {
                List<UserInformation> tempList =
                    _context.UsersInformation.Where(w => w.ClassroomId == Class.Id).ToList();
                Class.Students = tempList;
            }

            return Ok(classList);
        }
        [HttpGet]
        public async Task<ActionResult<List<ReturnedFacultyDto>>> GetAllStudent()
        {
            List<Faculty> faculties = _context.Faculty.ToList();
            List<ReturnedFacultyDto> resList = new List<ReturnedFacultyDto>();
            foreach (Faculty faculty in faculties)
            {
                List<Classroom> classList = _context.Classrooms.Where(w => w.FacultyId == faculty.Id).ToList();
                foreach (Classroom Class in classList)
                {
                    List<UserInformation> studentList = _context.UsersInformation.Where(w => w.ClassroomId == Class.Id).ToList();
                    Class.Students = studentList;
                }

                ReturnedFacultyDto tempDto = new ReturnedFacultyDto
                {
                    facultyId = faculty.Id,
                    facultyName = faculty.Name,
                    Classes = classList
                };
                resList.Add(tempDto);
            }
            return Ok(resList);
        }
    }
}

