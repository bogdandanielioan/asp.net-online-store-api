using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Enrolments.Dto
{
    public class CreateRequestEnrolment
    {
        public string IdCourse { get; set; }

        public DateTime Created { get; set; }
    }
}
