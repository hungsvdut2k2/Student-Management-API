using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class CourseEducationalProgram
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int EducationalProgramId { get; set; }
        [JsonIgnore]
        public EducationalProgram EducationalProgram { get; set; }
    }
}
