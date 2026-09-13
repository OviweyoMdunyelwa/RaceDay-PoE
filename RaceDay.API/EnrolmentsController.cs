using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Enrolments
        // Authenticated users can view enrolments
        [HttpGet]
        public async Task<IActionResult> GetEnrolments()
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

            var enrolments = await _context.Enrolments
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/Enrolments/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnrolment(int id)
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

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound(new
                {
                    message = "Enrolment not found."
                });
            }

            return Ok(enrolment);
        }

        // POST: api/Enrolments
        // Participant only
        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(Enrolment enrolment)
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

            if (role != "Participant")
            {
                return Forbid();
            }

            // Make sure the participant ID comes from the logged-in session
            enrolment.ParticipantID = userID.Value;
            enrolment.EnrolmentDate = DateTime.Now;

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == enrolment.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryID == enrolment.CategoryID);

            if (!categoryExists)
            {
                return BadRequest(new
                {
                    message = "The specified category does not exist."
                });
            }

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = enrolment.EnrolmentID },
                enrolment);
        }

        // PUT: api/Enrolments/1
        // Participant only
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrolment(
            int id,
            Enrolment updatedEnrolment)
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

            if (role != "Participant")
            {
                return Forbid();
            }

            var existingEnrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (existingEnrolment == null)
            {
                return NotFound(new
                {
                    message = "Enrolment not found."
                });
            }

            if (existingEnrolment.ParticipantID != userID.Value)
            {
                return Forbid();
            }

            existingEnrolment.EventID = updatedEnrolment.EventID;
            existingEnrolment.CategoryID = updatedEnrolment.CategoryID;
            existingEnrolment.RaceNumber = updatedEnrolment.RaceNumber;
            existingEnrolment.PaymentStatus = updatedEnrolment.PaymentStatus;

            await _context.SaveChangesAsync();

            return Ok(existingEnrolment);
        }

        // DELETE: api/Enrolments/1
        // Participant only
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
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

            if (role != "Participant")
            {
                return Forbid();
            }

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound(new
                {
                    message = "Enrolment not found."
                });
            }

            if (enrolment.ParticipantID != userID.Value)
            {
                return Forbid();
            }

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Enrolment deleted successfully."
            });
        }
    }
}