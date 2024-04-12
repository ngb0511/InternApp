using Base.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

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
            if (TempData.ContainsKey("ErrorMessage"))
            {
                // Lấy thông điệp lỗi từ TempData
                string errorMessage = TempData["ErrorMessage"] as string;

                // Xóa thông điệp lỗi từ TempData để không hiển thị lại ở lần chuyển hướng tiếp theo
                TempData.Remove("ErrorMessage");

                // Truyền thông điệp lỗi đến View
                ViewBag.ErrorMessage = errorMessage;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CheckUser(UserAssign userAssign)
        {            
            try
            {
                // Tạo một đối tượng HttpClient
                string baseUrl = "https://localhost:7083/api/UserAssign";
                string username = userAssign.UserName;
                string apiUrl = $"{baseUrl}?userName={username}";
                // Gửi yêu cầu GET đến API và nhận lại phản hồi
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Kiểm tra xem yêu cầu có thành công không (statusCode 200)
                if (response.IsSuccessStatusCode)
                {
                    /*// Đọc dữ liệu từ phản hồi
                    string responseData = await response.Content.ReadAsStringAsync();*/
                    return RedirectToAction("Index", "TimingPost");                   

                }
                TempData["ErrorMessage"] = "Username không tồn tại";
                return RedirectToAction("Index", "Home"); ;                
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed: " + ex.Message);
                return BadRequest();
            }            
        }
    }
}