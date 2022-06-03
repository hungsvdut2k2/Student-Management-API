namespace API.Models.DtoModels
{
    public class CreateCourseDto
    {
        public string Name { get; set; }
        public int Credits { get; set; }
        public int EducationalProgramId { get; set; }
    }
}
