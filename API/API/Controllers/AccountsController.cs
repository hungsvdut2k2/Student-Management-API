using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
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
using netcuoiky;
using OfficeOpenXml;

namespace API.Controllers
{

    [EnableCors("Allow CORS")]
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public static IWebHostEnvironment _enviroment;
        public AccountsController(ApplicationDbContext context, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _enviroment = environment;
        }
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LoginDto>> Register(RegisterDto request)
        {
            Classroom classroom = _context.Classroom.Where(findingClass => findingClass.Name == request.ClassName)
                .FirstOrDefault();
            var user = new User
            {
                UserId = request.UserId,
                Name = request.Name,
                Dob = request.Dob,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Gender = request.Gender,
                ImageUrl = string.Empty,
                Classroom = classroom
            };
            string Password = randomPassword();
            _context.User.Add(user);
            _context.SaveChanges();
            CreatePasswordHash(Convert.ToString(Password), out byte[] passwordHash, out byte[] passwordSalt);
            var account = new Account
            {
                Username = request.UserId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role,
                User = user
            };
            _context.Account.Add(account);
            await _context.SaveChangesAsync();
            LoginDto res = new LoginDto
            {
                Username = account.Username,
                Password = Password
            };
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult<List<ReturnedAccount>>> CreateAccounts(List<RegisterDto> requests)
        {
            List<Account> accounts = new List<Account>();
            List<User> userInformations = new List<User>();
            List<ReturnedAccount> result = new List<ReturnedAccount>();
            foreach (var request in requests)
            {
                Classroom classroom = _context.Classroom.Where(w => w.Name == request.ClassName).FirstOrDefault();
                if (classroom != null)
                {
                    var user = new User
                    {
                        UserId = request.UserId,
                        Name = request.Name,
                        Dob = request.Dob,
                        PhoneNumber = request.PhoneNumber,
                        Email = request.Email,
                        Gender = request.Gender,
                        ImageUrl = string.Empty,
                        Classroom = classroom
                    };
                    string Password = randomPassword();
                    CreatePasswordHash(Convert.ToString(Password), out byte[] passwordHash, out byte[] passwordSalt);
                    var newUser = new Account
                    {
                        Username = request.UserId,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        User = user,
                        Role = request.Role
                    };
                    var newLoginDto = new LoginDto
                    {
                        Username = newUser.Username,
                        Password = Password
                    };
                    var newAccount = new ReturnedAccount
                    {
                        Name = request.Name,
                        ClassName = request.ClassName,
                        Account = newLoginDto
                    };
                    result.Add(newAccount);
                    accounts.Add(newUser);
                    userInformations.Add(user);
                }
            }
            _context.User.AddRange(userInformations);
            _context.Account.AddRange(accounts);
            _context.SaveChanges();
            return Ok(result);
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
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            Account user = await _context.Account.Where(w => w.Username == request.Username).FirstAsync();
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
        [HttpPost("send-email/{userId}/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> SendEmail(string userId, string email)
        {
            //validation for email
            User user = _context.User.Find(userId);
            Account account = _context.Account.Where(w => w.UserId == userId).FirstOrDefault();
            if (user.Email == email)
            {
                Random random = new Random();
                int code = random.Next(123123, 999999);
                account.RefreshToken = code;
                _context.SaveChanges();
                EmailAccount emailAccount = new EmailAccount();
                string p = emailAccount.Password;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailAccount.Email);
                message.To.Add(new MailAddress(email));
                message.Subject = "change password";
                message.Body = "Write this given code on text box\n" + code + "\nThank you!";
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("viethungnguyen2002@gmail.com", p);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

                return Ok();
            }
            return BadRequest("Wrong Email");
        }
        [HttpPost("verify-token/{token}/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyToken(int token, string userId)
        {
            var account = _context.Account.Where(w => w.RefreshToken == token).FirstOrDefault();
            if (account == null)
            {
                return BadRequest("Wrong Code");
            }
            //reset the token
            if (account.UserId == userId)
            {
                account.RefreshToken = null;
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Wrong Code");
        }
        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<Account>> ForgotPassword(ForgotPasswordDto request)
        {
            Account account = _context.Account.Where(w => w.UserId == request.UserId).FirstOrDefault();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;
            _context.SaveChanges();
            return Ok(account);
        }

        private bool Verified(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Account user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId)
            };
            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        [HttpPut("reset-password")]
        [Authorize]
        public async Task<ActionResult<User>> ResetPassword(ResetPasswordDto request)
        {
            Account user = await _context.Account.Where(w => w.Username == request.Username).FirstAsync();
            if (user == null)
            {
                return NotFound();
            }

            if (Verified(request.OldPassword, user.PasswordHash, user.PasswordSalt) == true)
            {
                CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            return BadRequest("Wrong Password");
        }
        [Authorize(Roles = "Admin")]
        [Route("{username}")]
        [HttpDelete]
        public async Task<ActionResult<User>> Delete(string username)
        {
            Account user = await _context.Account.Where(account => account.Username == username).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            _context.Account.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class FileUpLoadAPI
        {
            public IFormFile files { get; set; }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload-file")]
        public async Task<ActionResult<List<LoginDto>>> uploadFile([FromForm] FileUpLoadAPI data)
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
                List<RegisterDto> registerList = new List<RegisterDto>();
                int rows = worksheet.Dimension.Rows;
                for (int i = 2; i <= rows; i++)
                {
                    RegisterDto request = new RegisterDto
                    {
                        ClassName = worksheet.Cells[i, 1].Text,
                        UserId = worksheet.Cells[i, 2].Text,
                        Name = worksheet.Cells[i, 3].Text,
                        Gender = worksheet.Cells[i, 4].Text != "Nữ",
                        Dob = worksheet.Cells[i, 5].Text,
                        Email = worksheet.Cells[i, 6].Text,
                        PhoneNumber = worksheet.Cells[i, 7].Text,
                        Role = "Student"
                    };
                    registerList.Add(request);
                }
                var result = await CreateAccounts(registerList);
                var castResult = (OkObjectResult)result.Result;
                var finalResult = (List<ReturnedAccount>)castResult.Value;
                return Ok(finalResult);
            }
            return BadRequest();
        }
    }
}

