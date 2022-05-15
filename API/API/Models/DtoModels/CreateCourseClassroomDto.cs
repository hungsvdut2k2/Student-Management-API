using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class CreateCourseClassroomDto
    {
        public string TeacherName { get; set; }
        public DateTime Schedule { get; set; } = DateTime.Now;
        public int CourseId { get; set; }
    }
}
