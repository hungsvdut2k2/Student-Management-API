using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<CourseClassroom> CourseClassrooms { get; set; }
        public ICollection<CourseEducationalProgram> CourseEducationalProgram { get; set; }
    }
}
