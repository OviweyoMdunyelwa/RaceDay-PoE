using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        // Both Organisers and Participants can view categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Categories/1
        // Both roles can view a specific category
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            return Ok(category);
        }

        // POST: api/Categories
        // Organiser only
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            string? role = HttpContext.Session.GetString("Role");
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return Forbid();
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == category.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryID },
                category);
        }

        // PUT: api/Categories/1
        // Organiser only
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            Category updatedCategory)
        {
            string? role = HttpContext.Session.GetString("Role");
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return Forbid();
            }

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (existingCategory == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == updatedCategory.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            existingCategory.EventID = updatedCategory.EventID;
            existingCategory.CategoryName = updatedCategory.CategoryName;
            existingCategory.DistanceKM = updatedCategory.DistanceKM;
            existingCategory.EntryFee = updatedCategory.EntryFee;
            existingCategory.AgeGroup = updatedCategory.AgeGroup;
            existingCategory.Description = updatedCategory.Description;

            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }

        // DELETE: api/Categories/1
        // Organiser only
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            string? role = HttpContext.Session.GetString("Role");
            int? userID = HttpContext.Session.GetInt32("UserID");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "You must be logged in."
                });
            }

            if (role != "Organiser")
            {
                return Forbid();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category deleted successfully."
            });
        }
    }
}