namespace API.Models.DatabaseModels
{
    public class EducationalProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseEducationalProgram> CourseEducationalProgram { get; set; }
    }
}
