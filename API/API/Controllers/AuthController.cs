using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(RegisterDto request)
        {
            Classroom classroom = await _context.Classrooms.FindAsync(request.ClassroomId);
            var userInformation = new UserInformation
            {
                UserId = request.UserInformationId,
                Name = string.Empty,
                Dob = DateTime.Now,
                PhoneNumber = string.Empty,
                Email = string.Empty,
                Gender = true,
                Classroom = classroom,
                CourseClassroomUserInformation = null
            };
            _context.UsersInformation.Add(userInformation);
            await _context.SaveChangesAsync();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role,
                UserInformation = userInformation
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Get(user.Id);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpGet("GetAccountById")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            User user = await _context.Users.Where(w => w.Username == request.Username).FirstAsync();
            if(user == null)
            {
                return NotFound();
            }
            if(Verified(request.Password, user.PasswordHash, user.PasswordSalt) == true)
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
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
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
        [HttpPut("ResetPassword")]
        public async Task<ActionResult<User>> ResetPassword(ResetPassowordDto request)
        {
            User user = await _context.Users.Where(w => w.Username == request.Username).FirstAsync();
            if(user == null)
            {
                return NotFound();
            }
            UserInformation userInformation = await _context.UsersInformation.Where(w => w.UserId == user.UserInformationId).FirstOrDefaultAsync();
            if(userInformation.PhoneNumber != request.PhoneNumber || userInformation.Email != request.Email)
            {
                return BadRequest("Uncertain Information");
            }
            CreatePasswordHash(request.newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete]
        public async Task<ActionResult<User>> Delete(int UserId)
        {
            User user = await _context.Users.FindAsync(UserId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
