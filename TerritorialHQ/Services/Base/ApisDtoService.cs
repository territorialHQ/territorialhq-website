using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TerritorialHQ_Library.DTO.Interface;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services.Base
{
    public class ApisDtoService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApisDtoService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _memoryCache = memoryCache;

#if (DEBUG)
            // Deactivate any SSL certificate validation in development because it never fucking works 

            HttpClientHandler handler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
#else
            _httpClient = new HttpClient();
#endif
            _httpClient.BaseAddress = new Uri(ConfigurationBinder.GetValue<string>(_configuration, "APIS_URI")! + "/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public virtual async Task<List<T>?> GetAllAsync<T>(string endpoint) where T : IDto
        {
            AddTokenHeader();

            List<T>? result = new();
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<List<T>>();
            }

            return result;
        }

        public async Task<T?> FindAsync<T>(string endpoint, string id) where T : IDto
        {
            AddTokenHeader();

            T? result = default;
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint + "/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<T>();
            }

            return result;
        }

        [Authorize]
        public async Task<string?> Add<T>(string endpoint, T item) where T : IDto
        {
            AddTokenHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, item);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<string>();
            else
                throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        [Authorize]
        public async Task<bool> Update<T>(string endpoint, T item) where T : IDto
        {
            AddTokenHeader();

            var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<bool> Remove(string? endpoint, string? id)
        {
            AddTokenHeader();

            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint + "/" + id);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        protected void AddTokenHeader()
        {
            var token = _contextAccessor.HttpContext?.Request.Cookies["BearerToken"] ?? string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
