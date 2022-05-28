using API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace API.Models.DtoModels
{
    public class InformationDto
    {
        public string UserId { get; set; }
        public string Name  { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
    }
}
