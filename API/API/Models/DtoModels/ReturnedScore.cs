using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedScore
    {
        public string Student { get; set; }
        public Score Score { get; set; }
        public double TotalScore { get; set; }
    }
}
