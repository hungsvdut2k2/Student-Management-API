using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string ImageUrl { get; set; }
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Classroom Classroom { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string ClassroomId { get; set; }
    }
}
