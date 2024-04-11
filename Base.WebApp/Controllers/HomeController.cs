using Base.Domain.ViewModels;
using Base.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
//using System.Text.Json;

namespace Base.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> DummyCodeView()
        {
            var response = await _httpClient.GetAsync("https://localhost:7083/api/DummyCode/GetAllDummyCode");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DummyCodeVM>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(yourModels);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return View();
            }

            //return View();
        }

        public async Task<IActionResult> DummyCodeEdit(int id)
        {
            //var httpClient = _httpClient.CreateClient();
            var response = await _httpClient.GetAsync($"https://localhost:7083/api/DummyCode/GetDummyCodeById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModel = JsonConvert.DeserializeObject<DummyCodeVM>(responseData);
                return View(yourModel);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DummyCodeUpdate(DummyCodeVM dummyCodeVM)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(dummyCodeVM), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7083/api/DummyCode/UpdateDummyCode/{dummyCodeVM.Id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DummyCodeView"); // Redirect to index or any other action after successful update
            }
            else
            {
                // Handle error
                return View(dummyCodeVM); // Return to edit view with model if update fails
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}