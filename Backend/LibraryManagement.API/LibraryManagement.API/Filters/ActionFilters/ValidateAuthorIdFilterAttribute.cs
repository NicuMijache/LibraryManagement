using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LibraryManagement.API.Data;

namespace LibraryManagement.API.Filters.ActionFilters
{
    public class ValidateAuthorIdFilterAttribute : IAsyncActionFilter
    {
        private readonly LibraryContext _context;

        public ValidateAuthorIdFilterAttribute(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    context.Result = new NotFoundObjectResult(new { Message = $"Autorul cu ID-ul {id} nu a fost găsit." });
                    return;
                }

                context.HttpContext.Items["author"] = author;
            }

            await next();
        }
    }
}