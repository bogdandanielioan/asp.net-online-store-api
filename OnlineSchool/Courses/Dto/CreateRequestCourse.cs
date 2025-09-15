using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Courses.Dto
{
    public class CreateRequestCourse
    {
        public string Name { get; set; }
        public string Department { get; set; }
        // Mandatory: assign a teacher when creating a course
        public string TeacherId { get; set; }
    }
}
