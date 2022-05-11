namespace API.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserInformation> UserInformation { get; set; }
    }
}
