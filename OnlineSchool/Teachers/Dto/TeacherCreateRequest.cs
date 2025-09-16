namespace OnlineSchool.Teachers.Dto
{
    public class TeacherCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        // Plain password at registration time
        public string? Password { get; set; }
    }
}
