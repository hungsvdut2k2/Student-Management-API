﻿namespace API.Models.DtoModels
{
    public class ResetPasswordDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
    }
}
