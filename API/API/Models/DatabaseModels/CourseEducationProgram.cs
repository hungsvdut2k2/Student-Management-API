using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class CourseEducationProgram
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Course Course { get; set; }
        public string CourseId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public EducationalProgram EducationalProgram { get; set; }
        public string EducationalProgramId { get; set; }
    }
}
