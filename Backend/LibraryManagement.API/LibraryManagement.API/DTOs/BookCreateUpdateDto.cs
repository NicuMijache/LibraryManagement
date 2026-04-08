using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs
{
    public class BookCreateUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        // Trimitem DOAR ID-urile!
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}