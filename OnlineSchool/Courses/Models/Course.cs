using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Students.Models;

namespace OnlineSchool.Courses.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        public virtual List<Enrolment> EnrolledStudents { get; set; }
    }
}
