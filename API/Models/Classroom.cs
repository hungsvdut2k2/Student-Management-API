namespace API.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //1 -> n : 1 classroom has too many students
        public List<UserInformation> UserInformations { get; set; }
    }
}
