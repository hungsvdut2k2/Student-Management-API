namespace API.Models.DatabaseModels
{
    public class Schedule
    {
        public int Id { get; set; }
        public int dateInWeek { get; set; }
        public int startPeriod { get; set; }
        public int endPeriod { get; set; }
        public string Room { get; set; }
        public CourseClassroom CourseClassroom { get; set; }
        public string CourseClassId { get; set; }
        public string ConvertNumberToDate()
        {
            var WeekDays = new Dictionary<int, string>()
            {
                {1, "Thứ Hai"},
                {2, "Thứ Ba"},
                {3, "Thứ Tư"},
                {4, "Thứ Năm"},
                {5, "Thứ Sáu"},
                {6, "Thứ Bảy"},
                {7, "Chủ Nhật"}
            };
            return WeekDays[dateInWeek];
        }

        public static int ConvertDateToNumber(string Date)
        {
            var WeekDays = new Dictionary<string, int>()
            {
                {"Thứ Hai", 1},
                {"Thứ Ba", 2},
                {"Thứ Tư", 3},
                {"Thứ Năm", 4},
                {"Thứ Sáu", 5},
                {"Thứ Bảy", 6},
                {"Chủ Nhật", 7},
            };
            return WeekDays[Date];
        }
    }
}
