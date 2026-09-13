using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    public class ResultsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ResultsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            _httpClient.BaseAddress = new Uri("https://localhost:7066/");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Results");

                if (response.IsSuccessStatusCode)
                {
                    var results =
                        await response.Content.ReadFromJsonAsync<List<ResultViewModel>>();

                    return View(results ?? new List<ResultViewModel>());
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewBag.ErrorMessage =
                        "Results require you to be logged in.";

                    return View(new List<ResultViewModel>());
                }

                ViewBag.ErrorMessage =
                    "Unable to load race results from the RaceDay API.";

                return View(new List<ResultViewModel>());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage =
                    "Unable to connect to RaceDay API.";

                return View(new List<ResultViewModel>());
            }
        }
    }

    public class ResultViewModel
    {
        public int ResultID { get; set; }

        public int EnrolmentID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public TimeSpan? ChipTime { get; set; }

        public int? PositionOverall { get; set; }

        public int? PositionCategory { get; set; }

        public decimal? Pace { get; set; }

        public string? Status { get; set; }
    }
}