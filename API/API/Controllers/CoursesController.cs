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
using OfficeOpenXml;

namespace API.Controllers
{

    [EnableCors("Allow CORS")]
    [Route("api/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public static IWebHostEnvironment _enviroment;

        public CoursesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }

        // GET: api/Courses
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Course>> GetCourse(string id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> UpdateCourse(CreateCourseDto request)
        {
            Course course = _context.Courses.Find(request.CourseId);
            if (course == null)
            {
                return NotFound();
            }

            course.requiredCourseId = request.requiredCourseId;
            course.Credits = request.Credits;
            course.Name = request.Name;
            course.isAvailable = request.isAvailable;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
            return Ok(course);
        }
        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> PostCourse(CreateCourseDto request)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }

            var course = new Course
            {
                CourseId = request.CourseId,
                Name = request.Name,
                Credits = request.Credits,
                requiredCourseId = request.requiredCourseId
            };
            _context.Courses.Add(course);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.CourseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }
        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(string id)
        {
            return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<Course>>> GetAvailableCourse(string userId)
        {
            User user = _context.User.Find(userId);
            Classroom classroom = _context.Classroom.Find(user.ClassroomId);
            List<CourseEducationProgram> courseEducationPrograms =
                _context.CourseEducationProgram.Where(w => w.EducationalProgramId == classroom.EducationalProgramId)
                    .ToList();
            //check course in educational program
            List<Course> courseInEducationalProgram = new List<Course>();
            foreach (var item in courseEducationPrograms)
            {
                Course course = _context.Courses.Find(item.CourseId);
                courseInEducationalProgram.Add(course);
            }
            //check available course
            List<Course> availableCourses = new List<Course>();
            foreach (var item in courseInEducationalProgram)
            {
                if (item.isAvailable == true)
                {
                    availableCourses.Add(item);
                }
            }
            //check completed course
            List<Course> unfinishedCourses = new List<Course>();
            List<UserCourse> completedCourses = _context.UserCourse.Where(w => w.UserId == userId).ToList();
            List<Course> finalList = new List<Course>();
            foreach (var item in availableCourses)
            {
                UserCourse completedCourse = _context.UserCourse
                    .Where(w => w.UserId == userId && w.CourseId == item.CourseId).FirstOrDefault();
                if(completedCourse == null)
                    unfinishedCourses.Add(item);
            }
            //check requirement 
            foreach (var course in unfinishedCourses)
            {
                if (course.requiredCourseId == string.Empty)
                {
                    finalList.Add(course);
                }
                else
                {
                    bool flag = false;
                    foreach (var item in completedCourses)
                    {
                        if (course.CourseId == item.CourseId)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag == true)
                    {
                        finalList.Add(course);
                    }
                }
            }
            return Ok(finalList);
        }

        public class FileUpLoadAPI
        {
            public IFormFile files { get; set; }
        }

        [HttpPost("upload-file")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CreateCourseDto>>> UploadTask([FromForm] FileUpLoadAPI data)
        {
            //download file from client
            if (data.files.Length > 0)
            {
                if (!Directory.Exists(_enviroment.WebRootPath + "\\Download\\"))
                {
                    Directory.CreateDirectory(_enviroment.WebRootPath + "\\Download\\");
                }

                using (FileStream fileStream =
                       System.IO.File.Create(_enviroment.WebRootPath + "\\Download\\" + data.files.FileName))
                {
                    data.files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                //work with excel file
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(_enviroment.WebRootPath + "\\Download\\" + data.files.FileName);
                ExcelPackage excelPackage = new ExcelPackage(fileInfo);
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                List<CreateCourseDto> registerList = new List<CreateCourseDto>();
                List<Course> courses = new List<Course>();
                int rows = worksheet.Dimension.Rows;
                for (int i = 2; i <= rows; i++)
                {
                    var newCourse = new CreateCourseDto
                    {
                        CourseId = worksheet.Cells[i, 3].Text,
                        Name = worksheet.Cells[i,4].Text,
                        Credits = Convert.ToDouble(worksheet.Cells[i, 5].Text),
                        requiredCourseId = worksheet.Cells[i, 6].Text
                    };
                    registerList.Add(newCourse);
                }
                foreach (var item in registerList)
                {
                    var course = new Course
                    {
                        CourseId = item.CourseId,
                        Name = item.Name,
                        Credits = item.Credits,
                        requiredCourseId = item.requiredCourseId,
                        isAvailable = true
                    };
                    courses.Add(course);
                }
                _context.Courses.AddRange(courses);
                _context.SaveChangesAsync();
                return Ok(registerList);
            }

            return BadRequest();
        }
    }
}