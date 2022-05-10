using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string TeacherName { get; set; }
        public DateTime Schedule { get; set; }

        //Add User Information and Course relationship : n -> n
        public List<UserInformation> UserInformation { get; set; }
        public List<EducationalProgram> EducationalProgram { get; set; }
    }
}
