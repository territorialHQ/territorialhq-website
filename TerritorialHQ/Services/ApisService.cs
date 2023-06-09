using MySqlX.XDevAPI;
using System.Drawing.Printing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TerritorialHQ.Services
{
    public class ApisService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApisService(IHttpContextAccessor contextAccessor, IConfiguration configuration) 
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;

#if (DEBUG)
            // Deactivate any SSL certificate validation in development because it never fucking works 

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _httpClient = new HttpClient(handler);
#else
            _httpClient = new HttpClient();
#endif
            _httpClient.BaseAddress = new Uri(ConfigurationBinder.GetValue<string>(_configuration, "APIS_URI"));
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            AddTokenHeader();

            List<T> result = new List<T>();
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<T>>();
            }

            return result;
        }

        public async Task<T> FindAsync<T>(string endpoint, string id)
        {
            AddTokenHeader();

            T result = default(T);
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<T>();
            }

            return result;
        }

        public async Task<bool> Update<T>(string endpoint, T item)
        {
            AddTokenHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, item);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        private void AddTokenHeader()
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["BearerToken"];
            if (token == null)
                throw new UnauthorizedAccessException();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
