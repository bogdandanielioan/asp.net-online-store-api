using System.ComponentModel.DataAnnotations;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.Students.Dto
{
    public class DtoStudentView
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Range(1, 120)]
        public int Age { get; set; }

        public DateTime UpdateData { get; set; }

        public List<Book> StudentBooks { get; set; } = new List<Book>();

        public StudentCard MyCardNumber { get; set; }

        public List<DtoCourseViewForStudents> MyCourses { get; set; } = new List<DtoCourseViewForStudents>();

    }
}
