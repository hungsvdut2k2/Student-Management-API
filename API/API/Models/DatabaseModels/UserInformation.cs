using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class UserInformation
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Classroom Classroom { get; set; }
        [JsonIgnore]
        public string ClassroomId { get; set; }
        [JsonIgnore]
        public ICollection<CourseClassroomUserInformation> CourseClassroomUserInformation { get; set; }
    }
}
