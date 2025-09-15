using System;

namespace OnlineSchool.Teachers.Dto
{
    public class DtoTeacherView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Role { get; set; }
    }
}