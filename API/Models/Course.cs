namespace API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        //n -> n:  1 course contain students 
        // and 1 student can attend to courses 
        public List<UserInformation> UserInformations { get; set; }
    }
}
