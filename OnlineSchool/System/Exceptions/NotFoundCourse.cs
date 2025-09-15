namespace OnlineSchool.System.Exceptions
{
    public class NotFoundCourse : Exception
    { 
        public NotFoundCourse(string? message):base(message) { }
    }
}
