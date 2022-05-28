using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        [JsonIgnore]
        public List<CourseClassroom> CourseClassrooms { get; set; }
        [JsonIgnore]
        public ICollection<CourseEducationalProgram> CourseEducationalProgram { get; set; }
    }
}
