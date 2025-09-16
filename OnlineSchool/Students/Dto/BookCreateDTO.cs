using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Students.Dto
{
    public class BookCreateDTO
    {
        public string Name { get; set; } = string.Empty;

        public DateTime Created_at { get; set; }
    }
}
