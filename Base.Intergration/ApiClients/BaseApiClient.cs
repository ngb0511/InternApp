using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Base.Intergration.ApiClients
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiAddress = "https://localhost:7083/";
        //private readonly string _apiAddress = "https://localhost:7253/";
        //private readonly string _apiAddress = "http://172.17.0.94:612/";
        //private readonly string _apiAddress = "http://172.17.0.88:412/";

        public BaseApiClient
        (
            IHttpClientFactory httpClientFactory
        )
        {
            _httpClientFactory = httpClientFactory;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiAddress);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new InvalidOperationException("Response body is empty or null.");
                    }

                    T deserializedObject = JsonConvert.DeserializeObject<T>(responseBody) ?? throw new ArgumentException("Deserialization resulted in a null object.");

                    return deserializedObject;
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"HTTP request failed: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"JSON deserialization failed: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Invalid operation: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred during processing: " + ex.Message);
                }
            }
        }

        public async Task<List<T>> GetListAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiAddress);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new InvalidOperationException("Response body is empty or null.");
                    }

                    var data = JsonConvert.DeserializeObject<List<T>>(responseBody) ?? throw new ArgumentException("Deserialization resulted in a null object.");

                    return data;
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"HTTP request failed: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"JSON deserialization failed: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Invalid operation: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred during processing: " + ex.Message);
                }
            }
        }

        protected async Task<TResponse> AddAsync<TResponse, T>(string url, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiAddress);

                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new InvalidOperationException("Response body is empty or null.");
                    }

                    TResponse deserializedObject = JsonConvert.DeserializeObject<TResponse>(responseBody) ?? throw new ArgumentException("Deserialization resulted in a null object.");

                    return deserializedObject;
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"HTTP request failed: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"JSON deserialization failed: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Invalid operation: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred during processing: " + ex.Message);
                }
            }
        }

        public async Task DeleteAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiAddress);

                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"HTTP request failed: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    throw new JsonException($"JSON deserialization failed: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Invalid operation: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred during processing: " + ex.Message);
                }
            }
        }

        protected async Task<bool> AddFileAsync(string url, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    using var client = new HttpClient();
                    client.BaseAddress = new Uri(_apiAddress);

                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;

                    using var form = new MultipartFormDataContent();
                    using var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(fileContent, "file", file.FileName);

                    HttpResponseMessage response = await client.PostAsync(url, form);

                    return response.IsSuccessStatusCode;
                }
                return false;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred during processing: " + ex.Message);
            }
        }

        protected async Task<TResponse> AddWithFileAsync<TResponse, T>(string url, T data, IFormFile file)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_apiAddress);

                string json = JsonConvert.SerializeObject(data);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                var form = new MultipartFormDataContent();
                var fileContent = new StreamContent(memoryStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                form.Add(httpContent);
                form.Add(fileContent, "file", file.FileName);

                HttpResponseMessage response = await client.PostAsync(url, form);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    throw new InvalidOperationException("Response body is empty or null.");
                }
                TResponse deserializedObject = JsonConvert.DeserializeObject<TResponse>(responseBody) ?? throw new ArgumentException("Deserialization resulted in a null object.");

                return deserializedObject;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"JSON deserialization failed: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Invalid operation: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred during processing: " + ex.Message);
            }
        }
    }
}
