namespace API.Models.DatabaseModels
{
    public class UserCourseClassroom
    {
        public User User { get; set; }
        public string UserId { get; set; }
        public CourseClassroom CourseClassroom { get; set; }
        public string CourseClassroomId { get; set; }
    }
}
