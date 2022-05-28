using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedFacultyDto
    {
        public string facultyId { get; set; }
        public string facultyName { get; set; }
        public List<Classroom> Classes { get; set; }
    }
}
