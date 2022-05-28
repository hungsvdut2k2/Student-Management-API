using System.ComponentModel.DataAnnotations;

namespace API.Models.DatabaseModels
{
    public class Faculty
    {
        [Key]
        public string FacultyId { get; set; }
        public string Name { get; set; }
        List<Classroom> Classrooms { get; set; }
    }
}
