using OnlineSchool.Students.Dto;

namespace OnlineSchool.Courses.Dto
{
    public class DtoCourseView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public List<DtoStudentViewForCourse> EnrolledStudents { get; set; }

    }
}
