using API.Models.DatabaseModels;

namespace API.Models.DtoModels
{
    public class ReturnedCourseClassroomDto
    {
        public CourseClassroom CourseClassroom { get; set; }
        public List<Schedule> Schedule { get; set; }
        public int NumberOfRegisteredStudent { get; set; }
    }
}
