using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public EnrolmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7066/");
        }

        // GET: /Enrolments
        public async Task<IActionResult> Index()
        {
            try
            {
                var enrolments =
                    await _httpClient.GetFromJsonAsync<List<EnrolmentViewModel>>(
                        "api/Enrolments");

                return View(enrolments ?? new List<EnrolmentViewModel>());
            }
            catch
            {
                ViewBag.ErrorMessage =
                    "Unable to connect to RaceDay API.";

                return View(new List<EnrolmentViewModel>());
            }
        }
    }

    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public int ParticipantID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string? RaceNumber { get; set; }

        public string? PaymentStatus { get; set; }
    }
}