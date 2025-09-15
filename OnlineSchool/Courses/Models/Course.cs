using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Teachers.Models;

namespace OnlineSchool.Courses.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        // Relation: many courses to one teacher
        public string? TeacherId { get; set; }
        public virtual Teacher? Teacher { get; set; }

        public virtual List<Enrolment> EnrolledStudents { get; set; }
    }
}
