using System;

namespace OnlineSchool.Teachers.Dto
{
    public class DtoTeacherView
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Role { get; set; }
    }
}
