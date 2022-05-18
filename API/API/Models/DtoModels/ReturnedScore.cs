using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedScore
    {
        public string information { get; set; }
        public Score score { get; set; }
        public double totalScore { get; set; }
    }
}
