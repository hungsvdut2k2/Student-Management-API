using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserInformation
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Faculty { get; set; }
        public User User { get; set; }
        public Classroom Classroom { get; set; }
        public int ClassroomId { get; set; }
        public List<Course> Courses { get; set; }
    }
}
