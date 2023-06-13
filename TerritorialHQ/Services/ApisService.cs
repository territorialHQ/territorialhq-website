using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services
{
    public class ApisService
    {
        protected readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApisService(IHttpContextAccessor contextAccessor, IConfiguration configuration) 
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;

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
            _httpClient.BaseAddress = new Uri(ConfigurationBinder.GetValue<string>(_configuration, "APIS_URI")!);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<List<T>?> GetAllAsync<T>(string endpoint) where T : IEntity
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

        public async Task<T?> FindAsync<T>(string endpoint, string id) where T : IEntity
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

        public async Task<bool> Add<T>(string endpoint, T item) where T : IEntity
        {
            AddTokenHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, item);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                throw new Exception(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<bool> Update<T>(string endpoint, T item) where T : IEntity
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

        public async Task<bool> Remove<T>(string? endpoint, string? id) where T : IEntity
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
            var token = _contextAccessor.HttpContext?.Request.Cookies["BearerToken"] ?? String.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
