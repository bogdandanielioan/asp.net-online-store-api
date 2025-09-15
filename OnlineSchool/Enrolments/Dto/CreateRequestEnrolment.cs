using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Enrolments.Dto
{
    public class CreateRequestEnrolment
    {
        public int IdCourse { get; set; }

        public DateTime Created { get; set; }
    }
}
