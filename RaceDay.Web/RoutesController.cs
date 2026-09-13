using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class RoutesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}