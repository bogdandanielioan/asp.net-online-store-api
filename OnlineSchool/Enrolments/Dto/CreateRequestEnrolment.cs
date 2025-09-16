using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Enrolments.Dto
{
    public class CreateRequestEnrolment
    {
        public string IdCourse { get; set; } = string.Empty;

        public DateTime Created { get; set; }
    }
}
