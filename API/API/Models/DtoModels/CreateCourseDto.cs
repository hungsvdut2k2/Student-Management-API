namespace API.Models.DtoModels
{
    public class CreateCourseDto
    {
        public string CourseId { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string? requiredCourseId { get; set; }
    }
}
