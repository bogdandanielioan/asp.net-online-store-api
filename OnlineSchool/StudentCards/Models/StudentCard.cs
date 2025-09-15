using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineSchool.Students.Models;
using System.Text.Json.Serialization;

namespace OnlineSchool.StudentCards.Models
{
    public class StudentCard
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ForeignKey("IdStudent")]
        public string IdStudent { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }

        [Required]
        public string Namecard { get; set; }


    }
}
