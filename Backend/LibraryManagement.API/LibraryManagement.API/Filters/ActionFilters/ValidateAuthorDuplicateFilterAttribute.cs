using LibraryManagement.API.Data;
using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Filters.ActionFilters
{
    public class ValidateAuthorDuplicateFilterAttribute : IAsyncActionFilter
    {
        private readonly LibraryContext _context;

        public ValidateAuthorDuplicateFilterAttribute(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificăm dacă request-ul conține obiectul "author" trimis din Postman
            if (context.ActionArguments.TryGetValue("author", out var authorObj) && authorObj is Author author)
            {
                // Căutăm în baza de date dacă există deja
                var existaDeja = await _context.Authors.AnyAsync(a =>
                    a.FirstName.ToLower() == author.FirstName.ToLower() &&
                    a.LastName.ToLower() == author.LastName.ToLower());

                if (existaDeja)
                {
                    // Respingem direct cererea, Controller-ul nici măcar nu va fi deranjat
                    context.Result = new BadRequestObjectResult(new { Message = "Acest autor există deja în baza de date!" });
                    return;
                }
            }

            // Dacă nu există, lăsăm request-ul să treacă mai departe spre Controller
            await next();
        }
    }
}