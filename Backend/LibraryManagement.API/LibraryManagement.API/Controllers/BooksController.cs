using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Filters.ActionFilters;
using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // Folosim .Include() pentru ca JSON-ul să conțină și detaliile autorului/categoriei
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(ValidateBookIdFilterAttribute))]
        public IActionResult GetBookById(int id)
        {
            // Cartea vine gata "încărcată" cu autor și categorie de la filtrul nostru!
            var book = HttpContext.Items["book"] as Book;
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] BookCreateUpdateDto bookDto)
        {
            // Validăm dacă Autorul și Categoria chiar există în BD înainte să creăm cartea
            var authorExists = await _context.Authors.AnyAsync(a => a.Id == bookDto.AuthorId);
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == bookDto.CategoryId);

            if (!authorExists || !categoryExists)
            {
                return BadRequest("Autorul sau Categoria specificată nu există.");
            }

            // Mapăm (transferăm) datele din DTO în modelul real
            var newBook = new Book
            {
                Title = bookDto.Title,
                Price = bookDto.Price,
                AuthorId = bookDto.AuthorId,
                CategoryId = bookDto.CategoryId,
                IsAvailable = true // O carte nouă este din start disponibilă
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            // Returnăm adresa unde se poate vedea cartea nou creată
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateBookIdFilterAttribute))]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookCreateUpdateDto bookDto)
        {
            var bookToUpdate = HttpContext.Items["book"] as Book;

            // Validăm din nou relațiile
            if (!await _context.Authors.AnyAsync(a => a.Id == bookDto.AuthorId) ||
                !await _context.Categories.AnyAsync(c => c.Id == bookDto.CategoryId))
            {
                return BadRequest("Autorul sau Categoria specificată nu există.");
            }

            // Actualizăm câmpurile
            bookToUpdate!.Title = bookDto.Title;
            bookToUpdate.Price = bookDto.Price;
            bookToUpdate.AuthorId = bookDto.AuthorId;
            bookToUpdate.CategoryId = bookDto.CategoryId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(ValidateBookIdFilterAttribute))]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookToDelete = HttpContext.Items["book"] as Book;

            _context.Books.Remove(bookToDelete!);
            await _context.SaveChangesAsync();

            return Ok(bookToDelete);
        }
    }
}