using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedScore
    {
        public string Information { get; set; }
        public Score Score { get; set; }
        public double TotalScore { get; set; }
    }
}
