using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.Students.Dto
{
    public class DtoStudentView
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Email { get; set; }

        public int Age {  get; set; }

        public DateTime UpdateData { get; set; }

        public List<Book> StudentBooks { get; set; }

        public StudentCard MyCardNumber { get; set; } 

        public List<DtoCourseViewForStudents> MyCourses {  get; set; }

    }
}
