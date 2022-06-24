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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Cors;
using Account = API.Models.DatabaseModels.Account;

namespace API.Controllers
{
    [EnableCors("Allow CORS")]
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public static IWebHostEnvironment _enviroment;
        public UsersController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            User userInformation = await _context.User.FindAsync(id);
            Classroom classroom = _context.Classroom.Find(userInformation.ClassroomId);
            Faculty faculty = await _context.Faculty.FindAsync(classroom.FacultyId);
            EducationalProgram educationalProgram = _context.EducationalProgram.Find(classroom.EducationalProgramId);
            var returnedInformation = new
            {
                UserInformation = userInformation,
                ClassroomName = classroom.Name,
                EducationalProgram = educationalProgram,
                Faculty = faculty
            };
            return Ok(returnedInformation);
        }
        [HttpGet("class/{classroomId}")]
        public async Task<ActionResult<List<User>>> GetAllStudentInClass(string classroomId)
        {
            var studentList = _context.User.Where(w => w.ClassroomId == classroomId).ToList();
            return Ok(studentList);
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<ActionResult<List<Classroom>>> GetAllStudentInFaculty(string facultyId)
        {
            Faculty faculty = await _context.Faculty.FindAsync(facultyId);
            List<Classroom> classList = _context.Classroom.Where(w => w.FacultyId == faculty.FacultyId).ToList();
            foreach (var Class in classList)
            {
                List<User> tempList =
                    _context.User.Where(w => w.ClassroomId == Class.ClassroomId).ToList();
                Class.Students = tempList;
            }

            return Ok(classList);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(string userId, InformationDto request)
        {
            User newUserInformation = await _context.User.FindAsync(userId);
            newUserInformation.Name = request.Name;
            newUserInformation.Dob = request.Dob;
            newUserInformation.Email = request.Email;
            newUserInformation.Gender = request.Gender;
            newUserInformation.PhoneNumber = request.PhoneNumber;
            await _context.SaveChangesAsync();
            return Ok(newUserInformation);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("course-class/{UserId}")]
        public async Task<ActionResult<List<CourseClassroom>>> GetAllCourseClassroom(string UserId)
        {
            var classList = (from w in _context.UserCourseClassroom
                where w.UserId == UserId
                select w).ToList();
            var resCourseClassList = new List<CourseClassroom>();
            foreach (var courseClassroom in classList)
            {
                CourseClassroom findingClass = await _context.CourseClassroom.FindAsync(courseClassroom.CourseClassroomId);
                resCourseClassList.Add(findingClass);
            }
            return Ok(resCourseClassList);
        }
        private bool UserExists(string id)
        {
            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        public class FileUpLoadAPI
        {
            public IFormFile files { get; set; }
        }

        //[HttpPost("image-upload/{userId}")]
        //public async Task<ActionResult<string>> UploadImage([FromForm] FileUpLoadAPI objFiles, string userId)
        //{
        //     CloudinaryAccount cloudinaryAccount = new CloudinaryAccount();
        //    var account = new CloudinaryDotNet.Account(
        //    cloud: cloudinaryAccount.Cloud,
        //    apiKey: cloudinaryAccount.ApiKey,
        //    apiSecret: cloudinaryAccount.ApiSecret);


        //    Cloudinary cloudinary = new Cloudinary(account);
        //    cloudinary.Api.Secure = true;
        //    User userInformation = _context.User.Find(userId);
        //    try
        //    {
        //        if (objFiles.files.Length > 0)
        //        {
        //            if (!Directory.Exists(_enviroment.WebRootPath + "\\Upload\\"))
        //            {
        //                Directory.CreateDirectory(_enviroment.WebRootPath + "\\Upload\\");
        //            }

        //            using (FileStream fileStream =
        //                   System.IO.File.Create(_enviroment.WebRootPath + "\\Upload\\" + userInformation.UserId))
        //            {
        //                objFiles.files.CopyTo(fileStream);
        //                fileStream.Flush();
        //            }

        //            if (userInformation.ImageUrl != String.Empty)
        //            {
        //                var deletedParam = new DeletionParams(userInformation.UserId);
        //                cloudinary.Destroy(deletedParam);
        //            }

        //            var uploadParams = new ImageUploadParams()
        //            {
        //                File = new FileDescription(_enviroment.WebRootPath + "\\Upload\\" + userInformation.UserId),
        //                PublicId = userInformation.UserId
        //            };
        //            var uploadResult = cloudinary.Upload(uploadParams);
        //            System.IO.File.Delete(_enviroment.WebRootPath + "\\Upload\\" + userInformation.UserId);
        //            userInformation.ImageUrl = uploadResult.PublicId;
        //            _context.SaveChanges();
        //            return Ok(uploadResult.Url);
        //        }
        //        else
        //        {
        //            return "failed";
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        return e.Message.ToString();
        //    }
        //}
        [HttpGet("teacher/course-classroom/{teacherName}")]
        public async Task<ActionResult<List<CourseClassroom>>> GetAllClassesOfTeacher(string teacherName)
        {
            List<CourseClassroom> courseClassrooms = _context.CourseClassroom
                .Where(courseClass => courseClass.TeacherName == teacherName).ToList();
            return Ok(courseClassrooms);
        }
    }
}