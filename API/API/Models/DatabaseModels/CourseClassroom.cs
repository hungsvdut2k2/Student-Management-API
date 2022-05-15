using System.ComponentModel.DataAnnotations;

namespace API.Models.DatabaseModels
{
    public class CourseClassroom
    {
        [Key]
        public int CourseClassroomId { get; set; }
        public string TeacherName { get; set; }
        public DateTime Schedule { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public ICollection<CourseClassroomUserInformation> CourseClassroomUserInformation { get; set; }
    }
}
