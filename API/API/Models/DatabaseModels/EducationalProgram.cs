using Newtonsoft.Json;

namespace API.Models.DatabaseModels
{
    public class EducationalProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<CourseEducationalProgram> CourseEducationalProgram { get; set; }
    }
}
