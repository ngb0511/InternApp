using Azure.Core;
using Base.Domain.Models.TimingPost;
using Base.Domain.ViewModels;
using Base.Intergration.ApiClients;
using Base.Intergration.Constracts;
using Base.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace Base.WebApp.Controllers
{
    public class TimingPostController : Controller
    {
        private readonly ITimingPostApiClient _timingPostApiClient;
        private readonly HttpClient _httpClient;

        public TimingPostController(ITimingPostApiClient timingPostApiClient, IHttpClientFactory httpClientFactory) 
        {
            _timingPostApiClient = timingPostApiClient;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
                return View();
        }

      /*  public async Task<IActionResult> GetData()
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
        }*/

        public async Task<JsonResult> Create(TimingRequest request)
        {            
            var result = await _timingPostApiClient.Create(request);
            return Json(new {success = true, message = result.Message});
        }

        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var result = await _timingPostApiClient.GetById(id);
                var timingPost = new TimingPostVM
                {
                    Id = id,
                    Customer = result.Customer,
                    PostName = result.PostName,
                    PostStart = result.PostStart,
                    PostEnd = result.PostEnd,
                    CreatedDate = result.CreatedDate,
                    CreatedByName = result.CreatedByName,
                };
                return Json(new { success = true, data = timingPost });
            }catch (Exception ex)
            {
                return Json(new { hasError = true, message = $"Data not found! Error:{ex.Message}" });
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
         /* 
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
      }*/
    }
}
