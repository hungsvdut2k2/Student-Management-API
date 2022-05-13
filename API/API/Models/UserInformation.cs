using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Classroom Classroom { get; set; }
        [JsonIgnore]
        public int ClassroomId { get; set; }
        [JsonIgnore]
        public List<Course> Courses { get; set; }
    }
}
