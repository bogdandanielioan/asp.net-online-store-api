using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Courses.Dto
{
    public class DtoCourseViewForStudents
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;
    }
}
