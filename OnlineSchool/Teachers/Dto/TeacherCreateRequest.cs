namespace OnlineSchool.Teachers.Dto
{
    public class TeacherCreateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        // Plain password at registration time
        public string? Password { get; set; }
    }
}