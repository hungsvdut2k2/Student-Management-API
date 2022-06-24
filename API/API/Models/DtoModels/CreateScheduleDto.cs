namespace API.Models.DtoModels
{
    public class CreateScheduleDto
    {
        public int dateInWeek { get; set; }
        public int startPeriod { get; set; }
        public int endPeriod { get; set; }
        public string Room { get; set; }
        public string courseClassId { get; set; }
    }
}
