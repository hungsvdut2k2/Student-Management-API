using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class EducationalProgram
    {
        [Key]
        public string EducationalProgramId { get; set; }
        public string Name { get; set; }
    }
}
