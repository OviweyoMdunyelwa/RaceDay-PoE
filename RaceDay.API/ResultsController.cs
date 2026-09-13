using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET: api/Results
        // Public - anyone can view race results
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var results = await _context.Results
                .ToListAsync();

            return Ok(results);
        }

        // =========================================================
        // GET: api/Results/1
        // Public - anyone can view a specific result
        // =========================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResult(int id)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            return Ok(result);
        }

        // =========================================================
        // POST: api/Results
        // Organiser only
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> CreateResult(Result result)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

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

            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == result.EnrolmentID);

            if (!enrolmentExists)
            {
                return BadRequest(new
                {
                    message = "The specified enrolment does not exist."
                });
            }

            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }

        // =========================================================
        // PUT: api/Results/1
        // Organiser only
        // =========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(
            int id,
            Result updatedResult)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

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

            var existingResult = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (existingResult == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == updatedResult.EnrolmentID);

            if (!enrolmentExists)
            {
                return BadRequest(new
                {
                    message = "The specified enrolment does not exist."
                });
            }

            existingResult.EnrolmentID = updatedResult.EnrolmentID;
            existingResult.FinishTime = updatedResult.FinishTime;
            existingResult.ChipTime = updatedResult.ChipTime;
            existingResult.PositionOverall = updatedResult.PositionOverall;
            existingResult.PositionCategory = updatedResult.PositionCategory;
            existingResult.Pace = updatedResult.Pace;
            existingResult.Status = updatedResult.Status;

            await _context.SaveChangesAsync();

            return Ok(existingResult);
        }

        // =========================================================
        // DELETE: api/Results/1
        // Organiser only
        // =========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

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

            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            _context.Results.Remove(result);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Result deleted successfully."
            });
        }
    }
}