using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserInformation
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Gender { get; set; }
        public string Classroom { get; set; }
        public string Faculty { get; set; }

        //Add User Information and Course relationship : n -> n
        public List<Course> Courses { get; set; }
        //Add Classroom and User Information relationship : n -> 1
        public int ClassroomId { get; set; }
        //Add User Information and Educational Program Relationship : 1 -> 1
        public int EducationalProgramId { get; set; }
    }
}
