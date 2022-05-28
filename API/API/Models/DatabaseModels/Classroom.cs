using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class Classroom
    {
        [Key]
        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public int AcademicYear { get; set; }
        public List<UserInformation> Students { get; set; }
        [JsonIgnore]
        public Faculty Faculty { get; set; }
        [JsonIgnore]
        public string FacultyId { get; set; }
    }
}
