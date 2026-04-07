using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] 
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        // (1-to-Many)
        [JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}