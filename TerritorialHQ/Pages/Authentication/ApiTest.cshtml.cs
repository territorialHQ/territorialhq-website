using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using NuGet.Common;
using System.Drawing.Printing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Pages.Authentication
{
    public class ApiTestModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ApiTestModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGet()
        {

            var token = HttpContext.Request.Cookies["BearerToken"];
            if (token == null)
                throw new Exception("No token found.");

#if (DEBUG)
            // Deactivate any SSL certificate validation in development because it never fucking works 

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using HttpClient _client = new HttpClient(handler);
#else
            using HttpClient _client = new HttpClient();
#endif
            _client.BaseAddress = new Uri(ConfigurationBinder.GetValue<string>(_configuration, "APIS_URI"));

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            List<AppUser> result = new();
            HttpResponseMessage response = await _client.GetAsync("Users");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<AppUser>>(data);
            }

            return Page();
        }
    }
}
