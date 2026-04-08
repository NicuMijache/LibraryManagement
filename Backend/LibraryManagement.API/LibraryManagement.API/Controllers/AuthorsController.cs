using LibraryManagement.API.Data;
using LibraryManagement.API.Filters.ActionFilters;
using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(ValidateAuthorIdFilterAttribute))]
        public IActionResult GetAuthorById(int id)
        {
            var author = HttpContext.Items["author"] as Author;
            return Ok(author);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAuthorDuplicateFilterAttribute))]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
          
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateAuthorIdFilterAttribute))]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            var authorToUpdate = HttpContext.Items["author"] as Author;

            // Actualizăm ambele câmpuri
            authorToUpdate!.FirstName = author.FirstName;
            authorToUpdate.LastName = author.LastName;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(ValidateAuthorIdFilterAttribute))]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var authorToDelete = HttpContext.Items["author"] as Author;

            _context.Authors.Remove(authorToDelete!);
            await _context.SaveChangesAsync();

            return Ok(authorToDelete);
        }
    }
}