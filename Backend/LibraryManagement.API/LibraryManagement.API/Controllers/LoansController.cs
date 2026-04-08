using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models;
using LibraryManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        // Injectăm Noul Serviciu, NU mai injectăm baza de date direct!
        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> CreateLoan([FromBody] LoanCreateDto loanDto)
        {
            try
            {
                // Controller-ul doar deleagă munca către Serviciu
                var newLoan = await _loanService.ProcessNewLoanAsync(loanDto);
                return Ok(newLoan);
            }
            catch (Exception ex)
            {
                // Dacă Serviciul a aruncat o eroare (ex: cartea e deja dată), o prindem aici
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                var resultMessage = await _loanService.ReturnBookAsync(id);
                return Ok(new { message = resultMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}