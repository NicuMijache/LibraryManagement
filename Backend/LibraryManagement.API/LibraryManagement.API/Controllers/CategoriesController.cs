using LibraryManagement.API.Data;
using LibraryManagement.API.Filters.ActionFilters;
using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")] // Adresa va fi: api/categories
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LibraryContext _context;

        // Aici facem Dependency Injection: cerem baza de date de la .NET
        public CategoriesController(LibraryContext context)
        {
            _context = context;
        }


        // GET: api/categories (Citirea tuturor categoriilor)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }



        // GET: api/categories/5
        [HttpGet("{id}")]
        [TypeFilter(typeof(ValidateCategoryIdFilterAttribute))]
        public IActionResult GetCategoryById(int id)
        {
            // Categoria a fost deja găsită de filtru, o luăm direct din HttpContext
            var category = HttpContext.Items["category"] as Category;

            return Ok(category);
        }


        // POST: api/categories (Crearea unei categorii noi)
        [HttpPost]
        [TypeFilter(typeof(ValidateCategoryDuplicateFilterAttribute))]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }


        // PUT: api/categories/5 (Actualizarea unei categorii)
        [HttpPut("{id}")]
        [TypeFilter(typeof(ValidateCategoryIdFilterAttribute))]
        [TypeFilter(typeof(ValidateCategoryDuplicateFilterAttribute))]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            // 1. Luăm categoria extrasă deja de filtrul nostru din baza de date
            var categoryToUpdate = HttpContext.Items["category"] as Category;

            // 2. Actualizăm datele (la categorie avem doar numele de schimbat)
            categoryToUpdate!.Name = category.Name;

            // 3. Salvăm modificările
            await _context.SaveChangesAsync();

            // 204 NoContent este codul standard de succes pentru un Update în REST
            return NoContent();
        }

        // DELETE: api/categories/5 (Ștergerea unei categorii)
        [HttpDelete("{id}")]
        [TypeFilter(typeof(ValidateCategoryIdFilterAttribute))]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // 1. Luăm categoria validată de filtru
            var categoryToDelete = HttpContext.Items["category"] as Category;

            // 2. Îi spunem lui Entity Framework să o șteargă
            _context.Categories.Remove(categoryToDelete!);

            // 3. Salvăm modificările
            await _context.SaveChangesAsync();

            // Returnăm categoria ștearsă ca să o vadă și frontend-ul
            return Ok(categoryToDelete);
        }
    }
}