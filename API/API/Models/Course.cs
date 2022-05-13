using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CourseClassroom> CourseClassrooms { get; set; }
        public List<EducationalProgram> EducationalProgram { get; set; }
    }
}
