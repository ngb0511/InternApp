using Base.Data.Models;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net.Http;
using System.Text;
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

        public IActionResult DummyCodeView()
        {
            return View();
        }

        //public async Task<IActionResult> GetAll()
        //{
        //    //Response.Headers.Add("Access-Control-Allow-Origin", "*");

        //    var response = await _httpClient.GetAsync("https://localhost:7083/api/DummyCode/GetAllDummyCode");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseData = await response.Content.ReadAsStringAsync();
        //        var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DummyCodeVM>>(responseData, new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });
        //        return Json(yourModels);
        //    }
        //    else
        //    {
        //        // Xử lý khi gặp lỗi
        //        return View();
        //    }
        //}

        public async Task<IActionResult> GetNumTotalPages(int pageSize)
        {
            var response = await _httpClient.GetAsync("https://localhost:7083/api/DummyCode/GetAllDummyCode");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DummyCodeVM>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (yourModels != null)
                {
                    var listVM = yourModels.ToList();

                    var totalCount = listVM.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    return Json(totalPages);
                }

                return BadRequest();
            }
            else
                    {
                // Xử lý khi gặp lỗi
                return BadRequest();
            }
        }


        public async Task<IActionResult> GetData(int page, int pageSize)
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var response = await _httpClient.GetAsync($"https://localhost:7083/api/DummyCode/GetPagedDummyCode?page={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var yourModels = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DummyCodeVM>>(responseData, new JsonSerializerOptions
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

        public async Task<IActionResult> DummyCodeDetail(int id)
        {
            //var httpClient = _httpClient.CreateClient();
            var response = await _httpClient.GetAsync($"https://localhost:7083/api/DummyCode/GetDummyCodeById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseJson = System.Text.Json.JsonSerializer.Deserialize<DummyCodeVM>(responseData, new JsonSerializerOptions
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

        [HttpPost]
        public async Task<IActionResult> DummyCodeEdit(DummyCodeVM dummyCodeVM)
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

        [HttpPost]
        public async Task<IActionResult> DummyCodeAdd(DummyCodeVM dummyCodeVM)
        {
            dummyCodeVM.Id = 0;
            dummyCodeVM.TotalMapping = 0;
            dummyCodeVM.CreatedDate = DateTime.Now;
            dummyCodeVM.CreatedBy = 1;
            
            var jsonContent = new StringContent(JsonConvert.SerializeObject(dummyCodeVM), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7083/api/DummyCode/AddDummyCode", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DummyCodeView"); // Redirect to index or any other action after successful update
            }
            else
            {
                int statusCode = (int)response.StatusCode;
                // Handle error
                string responseBody = await response.Content.ReadAsStringAsync();

                if (statusCode == 400)
                {
                    responseBody = "Dữ liệu bị trùng";
                }

                return StatusCode(statusCode, new { message = "Thêm thất bại", responseBody = responseBody });
            }
        }

        public async Task<IActionResult> DummyCodeAddByExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    // Handle the case where no file was selected
                    return BadRequest("No file selected");
                }

                // Read the file content
                byte[] fileBytes; // Get your file bytes here

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                string apiUrl = "https://localhost:7083/api/DummyCode/ImportDummyCodeFromExcel/1";

                // Prepare the request content (file content)
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(fileBytes), "File", file.FileName);

                    // Make the request to the API endpoint
                    var response = await _httpClient.PostAsync(apiUrl, content);

                    return RedirectToAction("DummyCodeView");
                }
            }
            catch (Exception)
            {
                // Handle any exceptions
                // You may want to log the exception or show an error message
                return RedirectToAction("UploadError");
            }
        }

        public async Task<IActionResult> ExportExcel()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7083/api/DummyCode/ExportDummyCodeToExcel");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exported_DummyCode.xlsx");
            }
            else
            {
                // Handle error response
                return BadRequest("Failed to export data to Excel");
            }
        }

        public IActionResult UploadError()
        {
            // Return view or redirect to an error page
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DummyCodeDelete(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7083/api/DummyCode/DeleteDummyCode/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Xóa thành công, chuyển hướng đến trang khác
                return RedirectToAction("DummyCodeView");
            }
            else
            {
                // Xử lý lỗi nếu có
                var errorMessage = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại sau.";
                // Xử lý lỗi cụ thể nếu cần
                // errorMessage = await response.Content.ReadAsStringAsync();

                TempData["ErrorMessage"] = errorMessage; // Lưu thông báo lỗi vào TempData để hiển thị trong trang khác
                return RedirectToAction("DummyCodeView");
            }
        }

    }
}
