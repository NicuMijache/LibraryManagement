using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        [Range(0.01, 1000)] // Validăm ca prețul să fie între 0.01 și 1000
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [JsonIgnore]
        public List<Loan> Loans { get; set; } = new List<Loan>();
    }
}