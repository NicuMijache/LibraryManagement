using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        //(1-to-Many)
        [JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}