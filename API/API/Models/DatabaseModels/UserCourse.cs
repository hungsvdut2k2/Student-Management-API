namespace API.Models.DatabaseModels
{
    public class UserCourse
    {
        public User User { get; set; }
        public string UserId { get; set; }
        public Course Course { get; set; }
        public string CourseId { get; set; }
    }
}
