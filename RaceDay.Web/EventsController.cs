using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly HttpClient _httpClient;

        public EventsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7066/");
        }

        // EVENTS LIST
        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _httpClient.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events");

                return View(events ?? new List<EventViewModel>());
            }
            catch
            {
                ViewBag.ErrorMessage = "Unable to connect to RaceDay API.";
                return View(new List<EventViewModel>());
            }
        }

        // EVENT DETAILS
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var eventDetails = await _httpClient.GetFromJsonAsync<EventViewModel>(
                    $"api/Events/{id}");

                if (eventDetails == null)
                {
                    return NotFound();
                }

                return View(eventDetails);
            }
            catch
            {
                return NotFound();
            }
        }
    }

    public class EventViewModel
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        public string? EventName { get; set; }

        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        public string? Location { get; set; }

        public DateTime RegistrationDeadline { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}