using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class Score
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public UserInformation UserInformation { get; set; }
        [JsonIgnore]
        public string UserInformationId { get; set; }
        [JsonIgnore]
        public CourseClassroom CourseClassroom { get; set; }
        [JsonIgnore]
        public int CourseClassroomId { get; set; }
        public double ExcerciseScore { get; set; }
        public double MidTermScore { get; set; }
        public double FinalTermScore { get; set; }
        public double ExcerciseRate { get; set; }
        public double MidTermRate { get; set; }

        public double FinalTermRate { get; set; }
        public double CalScore()
        {
            return ExcerciseRate * ExcerciseScore + MidTermRate * MidTermScore + FinalTermRate * FinalTermScore;
        }
    }
}
