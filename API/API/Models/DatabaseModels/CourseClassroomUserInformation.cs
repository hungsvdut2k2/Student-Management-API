namespace API.Models.DatabaseModels
{
    public class CourseClassroomUserInformation
    {
        public int CourseClassId { get; set; }
        public CourseClassroom CourseClassroom { get; set; }
        public string UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
