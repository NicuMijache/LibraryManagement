using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Data;

namespace LibraryManagement.API.Filters.ActionFilters
{
    public class ValidateBookIdFilterAttribute : IAsyncActionFilter
    {
        private readonly LibraryContext _context;

        public ValidateBookIdFilterAttribute(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
            {
                // Aici e "magia": folosim .Include() pentru a lipi datele din celelalte tabele
                var book = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    context.Result = new NotFoundObjectResult(new { Message = $"Cartea cu ID-ul {id} nu a fost găsită." });
                    return;
                }

                context.HttpContext.Items["book"] = book;
            }

            await next();
        }
    }
}