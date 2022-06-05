using System.ComponentModel.DataAnnotations;

namespace API.Models.DatabaseModels
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string ImageUrl { get; set; }
        public Classroom Classroom { get; set; }
        public string ClassroomId { get; set; }
    }
}
