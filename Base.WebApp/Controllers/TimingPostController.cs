using Base.Domain.ViewModels;
using Base.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace Base.WebApp.Controllers
{
    public class TimingPostController : Controller
    {
        private readonly HttpClient _httpClient;

        public TimingPostController(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
                return View();
        }

        public async Task<IActionResult> GetData()
        {
            var response = await _httpClient.GetAsync("https://localhost:7083/api/TimingPost/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TimingPostVM>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Json(yourModels);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return View();
            }
        }

        public async Task<JsonResult> Create(TimingPost timingPost)
        {            
            HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(timingPost), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7083/api/TimingPost/Create", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseJson = System.Text.Json.JsonSerializer.Deserialize<Response>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Json(responseJson);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return Json("");
            }
        }

        public async Task<JsonResult> GetById(int id)
        {
            HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(id), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.GetAsync("https://localhost:7083/api/TimingPost/"+id);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseJson = System.Text.Json.JsonSerializer.Deserialize<TimingPostVM>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Json(responseJson);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return Json("");
            }
        }

        public async Task<JsonResult> Edit(TimingPost timingPost)
        {
            HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(timingPost), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://localhost:7083/api/TimingPost/Update?id=" + timingPost.Id, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseJson = System.Text.Json.JsonSerializer.Deserialize<Response>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Json(responseJson);
            }
            else
            {
                // Xử lý khi gặp lỗi
                return Json("");
            }
        }

        public async Task<IActionResult> GetTotalRecord()
        {
            var response = await _httpClient.GetAsync("https://localhost:7083/api/TimingPost/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TimingPostVM>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (yourModels != null)
                {
                    return Json(yourModels.Count());
                }
                return View();
            }
            else
            {
                // Xử lý khi gặp lỗi
                return View();
            }
        }
    }
}
