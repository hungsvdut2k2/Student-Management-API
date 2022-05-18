using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class CourseClassroom
    {
        [Key]
        public int CourseClassroomId { get; set; }
        public string TeacherName { get; set; }
        public DateTime Schedule { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        [JsonIgnore]
        public int CourseId { get; set; }
        [JsonIgnore]
        public ICollection<CourseClassroomUserInformation> CourseClassroomUserInformation { get; set; }
    }
}
