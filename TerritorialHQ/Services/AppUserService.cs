using Newtonsoft.Json;
using System.Net.Http;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services
{
    public class AppUserService : ApisService
    {
        public AppUserService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
        {
        }

        public async Task<List<AppUser>?> GetUsersInRoleAsync(AppUserRole role)
        {
            AddTokenHeader();

            List<AppUser>? result = new();
            HttpResponseMessage response = await _httpClient.GetAsync("AppUser/RoleUsers/" + (int)role);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<AppUser>>(jsonString);
            }

            return result;
        }
    }
}
