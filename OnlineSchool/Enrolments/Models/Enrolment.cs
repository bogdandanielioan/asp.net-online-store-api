using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineSchool.Students.Models;
using System.Text.Json.Serialization;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Enrolments.Models
{

    public class Enrolment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ForeignKey("IdCourse")]
        public string IdCourse { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }

        [ForeignKey("IdStudent")]
        public string IdStudent { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }
        
        [Required]
        public DateTime Created {  get; set; }
    }
}
