using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using OnlineSchool.Students.Models;

namespace OnlineSchool.Books.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("IdStudent")]
        public int IdStudent { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string description()
        {
            string t = "";
            t += "Id: " + Id + "\n";
            t += $"IdStudent: {IdStudent} \n";
            t += $"Name: {Name} \n";
            t += $"Created_at: {Created.ToString("dd.mm.yyyy")} \n";

            return t;
        }


    }
}
