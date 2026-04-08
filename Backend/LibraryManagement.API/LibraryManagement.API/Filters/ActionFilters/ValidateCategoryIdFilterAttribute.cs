using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LibraryManagement.API.Data;

namespace LibraryManagement.API.Filters.ActionFilters
{
    public class ValidateCategoryIdFilterAttribute : IAsyncActionFilter
    {
        private readonly LibraryContext _context;

        // Injectăm baza de date pentru a putea face verificarea
        public ValidateCategoryIdFilterAttribute(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificăm dacă ruta conține parametrul "id"
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
            {
                // Căutăm categoria în baza de date
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    //404 Not Found
                    context.Result = new NotFoundObjectResult(new { Message = $"Categoria cu ID-ul {id} nu a fost găsită." });
                    return;
                }

                //O salvăm în HttpContext pentru a o refolosi în Controller!
                context.HttpContext.Items["category"] = category;
            }

            // Mergem mai departe la acțiunea din Controller
            await next();
        }
    }
}