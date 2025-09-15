using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Students.Dto
{
    public class BookCreateDTO
    {
        public string Name { get; set; }

        public DateTime Created_at { get; set; }
    }
}
