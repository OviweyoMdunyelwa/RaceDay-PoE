using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public WeatherController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Weather
        [HttpGet]
        public async Task<IActionResult> GetWeather()
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

            var weather = await _context.Weather
                .ToListAsync();

            return Ok(weather);
        }

        // GET: api/Weather/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherById(int id)
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

            var weather = await _context.Weather
                .FirstOrDefaultAsync(w => w.WeatherID == id);

            if (weather == null)
            {
                return NotFound(new
                {
                    message = "Weather record not found."
                });
            }

            return Ok(weather);
        }

        // POST: api/Weather
        // Organiser only
        [HttpPost]
        public async Task<IActionResult> CreateWeather(
            RaceDay.API.Models.Weather weather)
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
                return StatusCode(403, new
                {
                    message = "Only Organisers can create weather records."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == weather.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            _context.Weather.Add(weather);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWeatherById),
                new { id = weather.WeatherID },
                weather);
        }

        // PUT: api/Weather/{id}
        // Organiser only
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeather(
            int id,
            RaceDay.API.Models.Weather updatedWeather)
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
                return StatusCode(403, new
                {
                    message = "Only Organisers can update weather records."
                });
            }

            var existingWeather = await _context.Weather
                .FirstOrDefaultAsync(w => w.WeatherID == id);

            if (existingWeather == null)
            {
                return NotFound(new
                {
                    message = "Weather record not found."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == updatedWeather.EventID);

            if (!eventExists)
            {
                return BadRequest(new
                {
                    message = "The specified event does not exist."
                });
            }

            existingWeather.EventID = updatedWeather.EventID;
            existingWeather.ForecastDate = updatedWeather.ForecastDate;
            existingWeather.Temperature = updatedWeather.Temperature;
            existingWeather.Conditions = updatedWeather.Conditions;
            existingWeather.WindSpeed = updatedWeather.WindSpeed;
            existingWeather.Humidity = updatedWeather.Humidity;

            await _context.SaveChangesAsync();

            return Ok(existingWeather);
        }

        // DELETE: api/Weather/{id}
        // Organiser only
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeather(int id)
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
                return StatusCode(403, new
                {
                    message = "Only Organisers can delete weather records."
                });
            }

            var weather = await _context.Weather
                .FirstOrDefaultAsync(w => w.WeatherID == id);

            if (weather == null)
            {
                return NotFound(new
                {
                    message = "Weather record not found."
                });
            }

            _context.Weather.Remove(weather);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Weather record deleted successfully."
            });
        }
    }
}