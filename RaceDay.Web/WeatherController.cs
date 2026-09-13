using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}