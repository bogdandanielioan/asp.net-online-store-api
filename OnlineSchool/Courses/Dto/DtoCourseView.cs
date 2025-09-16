using System.ComponentModel.DataAnnotations;
using OnlineSchool.Students.Dto;

namespace OnlineSchool.Courses.Dto
{
    public class DtoCourseView
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        public List<DtoStudentViewForCourse> EnrolledStudents { get; set; } = new List<DtoStudentViewForCourse>();

    }
}
