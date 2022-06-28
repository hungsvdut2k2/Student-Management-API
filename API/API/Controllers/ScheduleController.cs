using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/schedules")]
    [EnableCors("Allow CORS")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{courseClassId}")]
        public async Task<ActionResult<List<Schedule>>> Get(string courseClassId)
        {
            var schedule = _context.Schedule.Where(w => w.CourseClassId == courseClassId).ToList();
            if (schedule == null)
                return NotFound();
            return Ok(schedule);
        }

        [HttpPut("{scheduleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Schedule>> Update(CreateScheduleDto request, int scheduleId)
        {
            Schedule schedule = _context.Schedule.Find(scheduleId);
            if (schedule == null)
                return NotFound();
            CourseClassroom courseClassroom = _context.CourseClassroom.Find(request.courseClassId);
            var newSchedule = new Schedule
            {
                dateInWeek = request.dateInWeek,
                startPeriod = request.startPeriod,
                endPeriod = request.endPeriod,
                Room = request.Room,
                CourseClassroom = courseClassroom,
                CourseClassId = courseClassroom.CourseClassId,
            };
            schedule = newSchedule;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Schedule>> PostSchedule(CreateScheduleDto request)
        {
            CourseClassroom courseClassroom = _context.CourseClassroom.Find(request.courseClassId);
            if (courseClassroom == null)
                return BadRequest("Non exist course classroom");
            var newSchedule = new Schedule
            {
                dateInWeek = request.dateInWeek,
                startPeriod = request.startPeriod,
                endPeriod = request.endPeriod,
                Room = request.Room,
                CourseClassroom = courseClassroom,
                CourseClassId = courseClassroom.CourseClassId,
            };
            _context.Schedule.Add(newSchedule);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return Ok(newSchedule);
        }
    }
}
