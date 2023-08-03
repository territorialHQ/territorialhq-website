using Newtonsoft.Json;
using System.Net.Http;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class AppUserService : ApisDtoService
{
    public AppUserService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }

    public async Task<List<DTOAppUser>?> GetUsersInRoleAsync(AppUserRole role)
    {
        AddTokenHeader();

        List<DTOAppUser>? result = new();
        HttpResponseMessage response = await _httpClient.GetAsync("AppUser/RoleUsers/" + (int)role);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<List<DTOAppUser>>(jsonString);
        }

        return result;
    }

    public async Task<bool> TogglePublicAsync(string id)
    {
        AddTokenHeader();

        bool result = false;
        HttpResponseMessage response = await _httpClient.GetAsync("AppUser/Public/" + id);

        if (response.IsSuccessStatusCode)
            result = true;

        return result;
    }
}
