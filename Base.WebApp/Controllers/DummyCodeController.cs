using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;

namespace Base.WebApp.Controllers
{
    public class DummyCodeController : Controller
    {
        private readonly HttpClient _httpClient;

        public DummyCodeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

        }
        public IActionResult Index()
        {
            return View();
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
    }
}
