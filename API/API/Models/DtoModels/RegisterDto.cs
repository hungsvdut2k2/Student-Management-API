namespace API.Models.DtoModels
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string UserInformationId { get; set; }
        public int ClassroomId  { get; set; }
    }
}
