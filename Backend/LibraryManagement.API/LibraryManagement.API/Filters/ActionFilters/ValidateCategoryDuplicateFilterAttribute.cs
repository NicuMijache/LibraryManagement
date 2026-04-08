using LibraryManagement.API.Data;
using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Filters.ActionFilters
{
    public class ValidateCategoryDuplicateFilterAttribute : IAsyncActionFilter
    {
        private readonly LibraryContext _context;

        public ValidateCategoryDuplicateFilterAttribute(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Extragem obiectul "category" trimis din Postman
            if (context.ActionArguments.TryGetValue("category", out var categoryObj) && categoryObj is Category category)
            {
                // Căutăm dacă există deja o categorie cu același nume (ignorând literele mari/mici)
                var existaDeja = await _context.Categories.AnyAsync(c =>
                    c.Name.ToLower() == category.Name.ToLower());

                if (existaDeja)
                {
                    // Respingem cererea direct de la "ușă"
                    context.Result = new BadRequestObjectResult(new { Message = "Această categorie există deja în baza de date!" });
                    return;
                }
            }

            await next();
        }
    }
}