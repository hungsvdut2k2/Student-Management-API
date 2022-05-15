namespace API.Models.DatabaseModels
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<Classroom> Classrooms { get; set; }
    }
}
