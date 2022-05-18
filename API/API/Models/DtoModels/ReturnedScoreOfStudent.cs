using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedScoreOfStudent
    {
        public CourseClassroom CourseClassroom { get; set; }
        public Score score { get; set; }
        public double totalScore { get; set; }
    }
}
