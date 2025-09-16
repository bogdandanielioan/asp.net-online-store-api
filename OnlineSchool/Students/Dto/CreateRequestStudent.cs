using OnlineSchool.Books.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Students.Dto
{
    public class CreateRequestStudent
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Age { get; set; }
        
        public string? Password { get; set; }

    }
}
