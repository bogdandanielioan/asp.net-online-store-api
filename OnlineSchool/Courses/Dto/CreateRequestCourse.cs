using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Courses.Dto
{
    public class CreateRequestCourse
    {
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        // Mandatory: assign a teacher when creating a course
        public string TeacherId { get; set; } = string.Empty;
    }
}
