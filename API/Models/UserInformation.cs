namespace API.Models
{
    public class UserInformation
    {
        //information for displaying 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime Dob { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        //1 -> n : 1 classroom has too many students
        public Classroom Classroom { get; set; }
        public int ClassroomId { get; set; }
        //1 -> n: 1 student can attend into courses
        public List<Course> Courses { get; set; }
        public Falcuty Falcuty { get; set; }
        public int FalcutyId { get; set;}
    }
}
