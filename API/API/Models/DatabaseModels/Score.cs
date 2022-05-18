using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class Score
    {
        public int Id { get; set; }
        [JsonIgnore]
        public UserInformation UserInformation { get; set; }
        [JsonIgnore]
        public string UserInformationId { get; set; }
        [JsonIgnore]
        public CourseClassroom CourseClassroom { get; set; }
        [JsonIgnore]
        public int CourseClassroomId { get; set; }
        public double excerciseScore { get; set; }
        public double midTermScore { get; set; }
        public double finalTermScore { get; set; }
        public double excerciseRate { get; set; }
        public double midTermRate { get; set; } 
        
        public double finalTermRate { get; set; }
        public double calScore()
        {
            return excerciseRate * excerciseScore + midTermRate * midTermScore + finalTermRate * finalTermScore;
        }
    }
}
