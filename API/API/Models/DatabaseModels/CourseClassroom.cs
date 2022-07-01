using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class CourseClassroom
    {
        [Key]
        public string CourseClassId { get; set; }
        public string TeacherName { get; set; }
        public Boolean isComplete { get; set; }
        public int Capacity { get; set; }
        public Course Course { get; set; }
        public string CourseId { get; set; }
    }
}
