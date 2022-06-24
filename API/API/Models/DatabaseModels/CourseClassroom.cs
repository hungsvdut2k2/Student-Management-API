using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class CourseClassroom
    {
        [Key]
        public string CourseClassId { get; set; }
        public string TeacherName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Course Course { get; set; }
        public string CourseId { get; set; }
    }
}
