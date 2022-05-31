using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
    [Route("api/account")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<ActionResult<LoginDto>> Register(RegisterDto request)
        {
            Classroom classroom = await _context.Classrooms.FindAsync(request.ClassroomId);
            Faculty faculty = await _context.Faculty.FindAsync(classroom.FacultyId);
            List<Classroom> classInFaculty = _context.Classrooms.Where(w => w.FacultyId == faculty.FacultyId).ToList();
            int numberOfStudents = configId(faculty.FacultyId);
            foreach (var findingClassroom in classInFaculty)
            {
                if (findingClassroom == classroom)
                    break;
                numberOfStudents += (_context.UsersInformation.Where(w => w.ClassroomId == findingClassroom.ClassroomId)
                    .ToList()).Count;
            }
            numberOfStudents += (_context.UsersInformation.Where(w => w.ClassroomId == classroom.ClassroomId).ToList()).Count + 1;
            string studentId;
            if (numberOfStudents < 10)
            {
                studentId = "000" + Convert.ToString(numberOfStudents);
            }
            else if (numberOfStudents > 10 && numberOfStudents < 100)
            {
                studentId = "00" + Convert.ToString(numberOfStudents);
            }
            else if (numberOfStudents > 100 && numberOfStudents < 1000)
            {
                studentId = "0" + Convert.ToString(numberOfStudents);
            }
            else
            {
                studentId = Convert.ToString(numberOfStudents);
            }
            var userInformation = new UserInformation
            {
                UserId = faculty.FacultyId + Convert.ToString(classroom.AcademicYear) + studentId,
                Name = request.Name,
                Dob = DateTime.Now,
                PhoneNumber = string.Empty,
                Email = string.Empty,
                Gender = request.Gender,
                ImageUrl = string.Empty,
                Classroom = classroom,
                CourseClassroomUserInformation = null
            };
            string Password = randomPassword();
            _context.UsersInformation.Add(userInformation);
            await _context.SaveChangesAsync();
            CreatePasswordHash(Convert.ToString(Password), out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = faculty.FacultyId + Convert.ToString(classroom.AcademicYear) + studentId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role,
                UserInformation = userInformation
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            LoginDto res = new LoginDto
            {
                Username = user.Username,
                Password = Password
            };
            return Ok(res);
        }

        private string randomPassword()
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < 10; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("Not Found");
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            User user = await _context.Users.Where(w => w.Username == request.Username).FirstAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (Verified(request.Password, user.PasswordHash, user.PasswordSalt) == true)
            {
                string token = CreateToken(user);
                return Ok(token);
            }
            return BadRequest("Wrong Password");
        }
        private bool Verified(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim("role",user.Role),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserInformationId)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpPut("reset-password")]
        public async Task<ActionResult<User>> ResetPassword(ResetPassowordDto request)
        {
            User user = await _context.Users.Where(w => w.Username == request.Username).FirstAsync();
            if (user == null)
            {
                return NotFound();
            }
            UserInformation userInformation = await _context.UsersInformation.Where(w => w.UserId == user.UserInformationId).FirstOrDefaultAsync();
            if (userInformation.PhoneNumber != request.PhoneNumber || userInformation.Email != request.Email)
            {
                return BadRequest("Uncertain Information");
            }
            CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        [Route("{userId}")]
        [HttpDelete]
        public async Task<ActionResult<User>> Delete(int userId)
        {
            User user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private int configId(string facultyId)
        {
            if (facultyId == "102")
            {
                return 7;
            }
            else if (facultyId == "101")
            {
                return 9;
            }
            else if (facultyId == "103")
            {
                return 3;
            }
            else if (facultyId == "104")
            {
                return 1;
            }
            else if (facultyId == "105")
            {
                return 76;
            }
            else if (facultyId == "106")
            {
                return 10;
            }
            else if (facultyId == "107")
            {
                return 25;
            }
            else if (facultyId == "109" || facultyId == "110")
            {
                return 19;
            }
            else if (facultyId == "111")
            {
                return 21;
            }
            else if (facultyId == "117")
            {
                return 5;
            }
            else if (facultyId == "118")
            {
                return 26;
            }
            else
            {
                return 0;
            }
        }

    }
}