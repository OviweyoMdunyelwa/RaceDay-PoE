using Microsoft.AspNetCore.Mvc;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSession()
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? role = HttpContext.Session.GetString("Role");

            if (userID == null || role == null)
            {
                return Unauthorized(new
                {
                    message = "No active session."
                });
            }

            return Ok(new
            {
                message = "Active session found.",
                userID,
                role
            });
        }
    }
}