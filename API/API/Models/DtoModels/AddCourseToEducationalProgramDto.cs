namespace API.Models.DtoModels
{
    public class AddCourseToEducationalProgramDto
    {
        public string CourseId { get; set; }
        public string EducationalProgramId { get; set; }
        public int Semester { get; set; }
    }
}
