namespace API.Models.DtoModels
{
    public class ForgotPasswordDto
    {
        public int Code { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
