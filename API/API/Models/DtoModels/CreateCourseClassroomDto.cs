namespace API.Models.DtoModels
{
    public class CreateCourseClassroomDto
    {
        public string CourseClassId { get; set; }
        public string TeacherName { get; set; }
        public string CourseId { get; set; }
        public int Capacity { get; set; }
    }
}
