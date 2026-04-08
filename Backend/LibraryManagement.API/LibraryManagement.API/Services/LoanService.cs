using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Services
{
    // Clasa moștenește interfața ILoanService
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _context;

        public LoanService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans.Include(l => l.Book).ToListAsync();
        }

        public async Task<Loan> ProcessNewLoanAsync(LoanCreateDto loanDto)
        {
            var book = await _context.Books.FindAsync(loanDto.BookId);

            if (book == null)
                throw new Exception("Cartea cu acest ID nu există în bibliotecă.");

            if (!book.IsAvailable)
                throw new Exception("Ne pare rău, această carte este deja împrumutată de altcineva.");

            var newLoan = new Loan
            {
                BorrowerName = loanDto.BorrowerName,
                BookId = loanDto.BookId,
                LoanDate = DateTime.Now
            };

            book.IsAvailable = false;

            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();

            return newLoan;
        }

        public async Task<string> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new Exception("Fișa de împrumut nu a fost găsită.");

            if (loan.ReturnDate != null)
                throw new Exception("Această carte a fost deja returnată.");

            loan.ReturnDate = DateTime.Now;

            if (loan.Book != null)
            {
                loan.Book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return $"Cartea a fost returnată cu succes de {loan.BorrowerName}!";
        }
    }
}