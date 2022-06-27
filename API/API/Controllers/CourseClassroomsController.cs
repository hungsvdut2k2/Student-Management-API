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

    [EnableCors("Allow CORS")]
    [Route("api/course-classroom")]
    [ApiController]
    public class CourseClassroomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseClassroomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CourseClassrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnedCourseClassroomDto>>> GetCourseClassroom()
        {
            List<CourseClassroom> courseClassrooms = _context.CourseClassroom.ToList();
            List<ReturnedCourseClassroomDto> resList = new List<ReturnedCourseClassroomDto>();
            foreach (var courseClassroom in courseClassrooms)
            {
                var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
                var tempData = new ReturnedCourseClassroomDto()
                {
                    CourseClassroom = courseClassroom,
                    Schedule = schedule
                };
                resList.Add(tempData);
            }

            return Ok(resList);
        }

        // GET: api/CourseClassrooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnedCourseClassroomDto>> GetCourseClassroom(string id)
        {
          if (_context.CourseClassroom == null)
          {
              return NotFound();
          }
            var courseClassroom = await _context.CourseClassroom.FindAsync(id);

            if (courseClassroom == null)
            {
                return NotFound();
            }
            var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
            var result = new ReturnedCourseClassroomDto
            {
                CourseClassroom = courseClassroom,
                Schedule = schedule
            };
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<ReturnedCourseClassroomDto>>> GetAllCourseClassroomByCourse(string courseId)
        {
            IEnumerable<CourseClassroom> courseClassrooms =
                _context.CourseClassroom.Where(courseClass => courseClass.CourseId == courseId).ToList();
            List<ReturnedCourseClassroomDto> resList = new List<ReturnedCourseClassroomDto>();
            foreach (var courseClassroom in courseClassrooms)
            {
                var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
                var tempData = new ReturnedCourseClassroomDto()
                {
                    CourseClassroom = courseClassroom,
                    Schedule = schedule
                };
                resList.Add(tempData);
            }

            return Ok(resList);
        }
        // POST: api/CourseClassrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseClassroom>> PostCourseClassroom(CreateCourseClassroomDto request)
        {
          if (_context.CourseClassroom == null)
          {
              return Problem("Entity set 'ApplicationDbContext.CourseClassroom'  is null.");
          }
          var course = await _context.Courses.FindAsync(request.CourseId);
          if (course == null)
          {
              return NotFound();
          }
          var courseClassroom = new CourseClassroom
          {
              CourseClassId = request.CourseClassId,
              Course = course,
              TeacherName = request.TeacherName
          };
            _context.CourseClassroom.Add(courseClassroom);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseClassroomExists(courseClassroom.CourseClassId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourseClassroom", new { id = courseClassroom.CourseClassId }, courseClassroom);
        }

        // DELETE: api/CourseClassrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseClassroom(string id)
        {
            if (_context.CourseClassroom == null)
            {
                return NotFound();
            }
            var courseClassroom = await _context.CourseClassroom.FindAsync(id);
            if (courseClassroom == null)
            {
                return NotFound();
            }

            _context.CourseClassroom.Remove(courseClassroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseClassroomExists(string id)
        {
            return (_context.CourseClassroom?.Any(e => e.CourseClassId == id)).GetValueOrDefault();
        }

        [HttpPost("user/{userId}/{courseClassId}")]
        public async Task<ActionResult<UserCourseClassroom>> RegisterCourseClass(string userId, string courseClassId)
        {
            User userInformation = await _context.User.FindAsync(userId);
            CourseClassroom courseClass = await _context.CourseClassroom.FindAsync(courseClassId);
            //validation
            Course registeredCourse = _context.Courses.Where(w => w.CourseId == courseClass.CourseId).First();
            if (Validate(userInformation, registeredCourse))
            {
                UserCourseClassroom courseUserInformation = new UserCourseClassroom
                {
                    UserId = userId,
                    User = userInformation,
                    CourseClassroomId = courseClassId,
                    CourseClassroom = courseClass
                };
                Score score = new Score
                {
                    User = userInformation,
                    UserId = userId,
                    CourseClassroom = courseClass,
                    CourseClassroomId = courseClassId,
                    ExcerciseRate = 0.2,
                    MidTermRate = 0.3,
                    FinalTermRate = 0.5,
                    ExcerciseScore = 0,
                    MidTermScore = 0,
                    FinalTermScore = 0
                };
                _context.Score.Add(score);
                _context.UserCourseClassroom.Add(courseUserInformation);
                await _context.SaveChangesAsync();
                return Ok(courseUserInformation);
            }
            return BadRequest("Have Participated In This Course");
        }
        private bool Validate(User userInformation, Course registeredCourse)
        {
            List<UserCourseClassroom> courseClassroomUserInformation = _context.UserCourseClassroom.Where(w => w.UserId == userInformation.UserId).ToList();
            List<CourseClassroom> courseClassrooms = new List<CourseClassroom>();
            List<Course> courseList = new List<Course>();
            foreach (var courseClass in courseClassroomUserInformation)
            {
                CourseClassroom findingCourseClassroom = _context.CourseClassroom.Where(w => w.CourseClassId == courseClass.CourseClassroomId).FirstOrDefault();
                courseClassrooms.Add(findingCourseClassroom);
            }
            foreach (var courseClass in courseClassrooms)
            {
                Course findingCourse = _context.Courses.Where(w => w.CourseId == courseClass.CourseId).First();
                courseList.Add(findingCourse);
            }
            foreach (var course in courseList)
            {
                if (course == registeredCourse)
                    return false;
            }
            return true;
        }
    }
}
