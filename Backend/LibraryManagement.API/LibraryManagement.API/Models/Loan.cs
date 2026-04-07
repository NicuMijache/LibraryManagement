using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? BorrowerName { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        // Nu punem [Required] aici pentru că la momentul împrumutului nu știm când o va returna
        public DateTime? ReturnDate { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}