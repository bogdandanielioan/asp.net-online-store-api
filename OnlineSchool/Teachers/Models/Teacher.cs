using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Teachers.Models
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? Subject { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        // Role-based authorization: every teacher is an Admin by default
        public string? Role { get; set; }

        // Password storage (hashed). Null for legacy/seeds without password
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        // Accept plain password at creation time only; do not map to DB
        [NotMapped]
        public string? Password { get; set; }

        // Navigation: one teacher has many courses
        public virtual List<Course> Courses { get; set; } = new();
    }
}
