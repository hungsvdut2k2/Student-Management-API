namespace API.Models.DtoModels
{
    public class RegisterDto
    {
        public string ClassName { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string? Dob { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        
    }
}
