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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnedCourseClassroomDto>>> GetCourseClassroom()
        {
            List<CourseClassroom> courseClassrooms = _context.CourseClassroom.ToList();
            List<ReturnedCourseClassroomDto> resList = new List<ReturnedCourseClassroomDto>();
            foreach (var courseClassroom in courseClassrooms)
            {
                var teacher = _context.User.Find(courseClassroom.TeacherName);
                var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
                List<UserCourseClassroom> userCourseClassrooms = _context.UserCourseClassroom
                    .Where(userCourseClass => userCourseClass.CourseClassroomId == courseClassroom.CourseClassId)
                    .ToList();
                var tempData = new ReturnedCourseClassroomDto()
                {
                    CourseClassroom = courseClassroom,
                    Schedule = schedule,
                    NumberOfRegisteredStudent = userCourseClassrooms.Count(),
                    TeacherName = teacher.Name
                };
                resList.Add(tempData);
            }

            return Ok(resList);
        }

        // GET: api/CourseClassrooms/5
        [HttpGet("{id}")]
        [Authorize]
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

            var teacher = _context.User.Find(courseClassroom.TeacherName);
            var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
            List<UserCourseClassroom> userCourseClassrooms = _context.UserCourseClassroom
                .Where(userCourseClass => userCourseClass.CourseClassroomId == courseClassroom.CourseClassId)
                .ToList();
            var result = new ReturnedCourseClassroomDto
            {
                CourseClassroom = courseClassroom,
                Schedule = schedule,
                NumberOfRegisteredStudent = userCourseClassrooms.Count(),
                TeacherName = teacher.Name
            };
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnedCourseClassroomDto>>> GetAllCourseClassroomByCourse(string courseId)
        {
            IEnumerable<CourseClassroom> courseClassrooms =
                _context.CourseClassroom.Where(courseClass => courseClass.CourseId == courseId && courseClass.isComplete == false).ToList();
            List<ReturnedCourseClassroomDto> resList = new List<ReturnedCourseClassroomDto>();
            foreach (var courseClassroom in courseClassrooms)
            {
                List<UserCourseClassroom> userCourseClassrooms = _context.UserCourseClassroom
                    .Where(userCourseClass => userCourseClass.CourseClassroomId == courseClassroom.CourseClassId)
                    .ToList();
                var teacher = _context.User.Find(courseClassroom.TeacherName);
                var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassroom.CourseClassId).ToList();
                var tempData = new ReturnedCourseClassroomDto()
                {
                    CourseClassroom = courseClassroom,
                    Schedule = schedule,
                    NumberOfRegisteredStudent = userCourseClassrooms.Count(),
                    TeacherName = teacher.Name
                };
                resList.Add(tempData);
            }

            return Ok(resList);
        }
        // POST: api/CourseClassrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
              isComplete = false,
              TeacherName = request.TeacherName,
              Capacity = request.Capacity
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourseClassroom(string id)
        {
            var schedule = _context.Schedule.Where(w => w.CourseClassId == id);
            if (_context.CourseClassroom == null)
            {
                return NotFound();
            }
            var courseClassroom = await _context.CourseClassroom.FindAsync(id);
            if (courseClassroom == null)
            {
                return NotFound();
            }
            _context.Schedule.RemoveRange(schedule);
            _context.CourseClassroom.Remove(courseClassroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseClassroomExists(string id)
        {
            return (_context.CourseClassroom?.Any(e => e.CourseClassId == id)).GetValueOrDefault();
        }
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ReturnedCourseClassroomDto>> GetAllRegisteredCourseClassroom(string userId)
        {
            List<UserCourseClassroom> userCourseClassroom = _context.UserCourseClassroom
                .Where(userCourseClass => userCourseClass.UserId == userId).ToList();
            List<ReturnedCourseClassroomDto> registeredCourseClassroom = new List<ReturnedCourseClassroomDto>();
            foreach (var item in userCourseClassroom)
            {
                CourseClassroom courseClassroom = _context.CourseClassroom.Where(courseClass =>
                        courseClass.CourseClassId == item.CourseClassroomId && courseClass.isComplete == false)
                    .FirstOrDefault();
                List<UserCourseClassroom> userCourseClassrooms = _context.UserCourseClassroom
                    .Where(userCourseClass => userCourseClass.CourseClassroomId == courseClassroom.CourseClassId)
                    .ToList();
                List<Schedule> schedules = _context.Schedule.Where(schedule => schedule.CourseClassId == courseClassroom.CourseClassId).ToList();
                var tempData = new ReturnedCourseClassroomDto
                {
                    CourseClassroom = courseClassroom,
                    Schedule = schedules,
                    NumberOfRegisteredStudent = userCourseClassrooms.Count()
                };
                registeredCourseClassroom.Add(tempData);
            }
            return Ok(registeredCourseClassroom);
        }

        [HttpPut("finish-all-classes")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<CourseClassroom>> CompletedAllClasses()
        {
            List<CourseClassroom> courseClassrooms = _context.CourseClassroom.ToList();
            foreach (var item in courseClassrooms)
            {
                item.isComplete = true;
            }

            _context.SaveChanges();
            return courseClassrooms;
        }
        [HttpPost("user/{userId}/{courseClassId}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<UserCourseClassroom>> RegisterCourseClass(string userId, string courseClassId)
        {
            User userInformation = await _context.User.FindAsync(userId);
            CourseClassroom courseClass = await _context.CourseClassroom.FindAsync(courseClassId);
            //validation
            Course registeredCourse = _context.Courses.Where(w => w.CourseId == courseClass.CourseId).First();
            if (Validate(userInformation, registeredCourse))
            {
                if (checkConflictSchedule(userId, courseClassId))
                {
                    if (isFull(courseClassId) == false)
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

                    return BadRequest("This Classroom Is Full Now");
                }

                return BadRequest("Conflict Schedule");
            }
            return BadRequest("Have Participated In This Course");
        }

        [HttpDelete("user/{userId}/{courseClassId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UnregisterCourseClass(string userId, string courseClassId)
        {
            UserCourseClassroom userCourseClassroom = _context.UserCourseClassroom.Where(userCourseClass =>
                    userCourseClass.CourseClassroomId == courseClassId && userCourseClass.UserId == userId)
                .FirstOrDefault();
            Score score = _context.Score.Where(w => w.CourseClassroomId == courseClassId && w.UserId == userId).FirstOrDefault();
            if (userCourseClassroom == null)
                return NotFound();
            _context.UserCourseClassroom.Remove(userCourseClassroom);
            _context.Score.Remove(score);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return Ok();
        }
        private bool checkConflictSchedule(string userId, string courseClassId)
        {
            CourseClassroom findingCourseClassroom = _context.CourseClassroom.Find(courseClassId);
            List<Schedule> findingSchedules = _context.Schedule
                .Where(w => w.CourseClassId == findingCourseClassroom.CourseClassId).ToList();
            List<CourseClassroom> courseClassrooms = new List<CourseClassroom>();
            List<UserCourseClassroom> userCourseClassrooms =
                _context.UserCourseClassroom.Where(w => w.UserId == userId).ToList();
            foreach (var item in userCourseClassrooms)
            {
                var courseClassroom = _context.CourseClassroom.Find(item.CourseClassroomId);
                courseClassrooms.Add(courseClassroom);
            }

            List<Schedule> schedules = new List<Schedule>();
            foreach (var courseClassroom in courseClassrooms)
            {
                List<Schedule> tempSchedules =
                    _context.Schedule.Where(s => s.CourseClassId == courseClassroom.CourseClassId).ToList();
                schedules.AddRange(tempSchedules);
            }

            foreach (var item in findingSchedules)
            {
                foreach (var schedule in schedules)
                {
                    if (item.dateInWeek == schedule.dateInWeek)
                    {
                        if ((item.startPeriod >= schedule.startPeriod && item.startPeriod <= schedule.endPeriod) ||
                            (item.endPeriod >= schedule.startPeriod && item.endPeriod <= schedule.endPeriod))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
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

        private bool isFull(string courseClassId)
        {
            List<UserCourseClassroom> userCourseClassrooms = _context.UserCourseClassroom
                .Where(userCourseClass => userCourseClass.CourseClassroomId == courseClassId).ToList();
            CourseClassroom courseClassroom = _context.CourseClassroom.Find(courseClassId);
            if (userCourseClassrooms.Count == courseClassroom.Capacity)
            {
                return true;
            }

            return false;
        }
    }
}
