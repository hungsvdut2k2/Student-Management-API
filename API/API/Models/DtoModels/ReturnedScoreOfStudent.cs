using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedScoreOfStudent
    {
        public CourseClassroom CourseClassroom { get; set; }
        public Score Score { get; set; }
        public double TotalScore { get; set; }
    }
}
