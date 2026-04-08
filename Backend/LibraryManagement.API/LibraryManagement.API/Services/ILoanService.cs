using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models;

namespace LibraryManagement.API.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<Loan> ProcessNewLoanAsync(LoanCreateDto loanDto);
        Task<string> ReturnBookAsync(int loanId);
    }
}