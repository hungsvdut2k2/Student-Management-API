namespace API.Models.DtoModels
{
    public class ResetPasswordDto
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
